using SockNet.ClientSocket;
using SockNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    /// <inheritdoc/>
    public class SocketServer : ISocketServer
    {

        private ITcpServer _listener;
        private ITcpClient _tcpClient;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private IPAddress _ip;
        private int _port;
        private int _readerBuffer;
        private byte[] _readerStartDelimitator;
        private byte[] _readerEndDelimitator;
        private List<KeyValuePair<TcpClient, byte[]>> _dataReceivedList;
        private object _listLock = new object();
        private Reader _readerAlgorithm;
        private int _readerBytesToRead;
        private bool _listen;

        internal CancellationTokenSource Cts { get => _cts; }
        internal CancellationToken Token { get => _token; }
        internal ITcpServer Listener { get => _listener; }

        /// <summary>
        /// Ip where the server is listening.
        /// </summary>
        public IPAddress Ip { get => _ip; }

        /// <summary>
        /// Port where the server is listening.
        /// </summary>
        public int Port { get => _port; }

        private enum Reader{
            ReaderBufferBytes,
            ReaderNumberOfBytes,
            ReaderBytesWithDelimitators,
            ReaderBytesWithEndDelimitator
        }


        /// <summary>
        /// Creates the socket server instance
        /// </summary>
        public SocketServer() : this( ServiceLocator.Current.Get<ITcpServer>(), ServiceLocator.Current.Get<ITcpClient>()) { }

        internal SocketServer(ITcpServer listener, ITcpClient client)
        {
            _listener = listener;
            _tcpClient = client;
            _dataReceivedList = new List<KeyValuePair<TcpClient, byte[]>>();
        }

        /// <inheritdoc/>
        public void InitializeSocketServer(string ip, int port)
        {
            _ip = Utils.Conversor.StringToIPAddress(ip);
            _port = port;

            _cts = new CancellationTokenSource();
            _token = _cts.Token;

            _listener.CreateTcpListener(_ip, _port);
        }

        /// <inheritdoc/>
        public async Task StartListening()
        {
            _listener.Start();
            _listen = true;

            var client = default(TcpClient);

            while (!_token.IsCancellationRequested && _listen)
            {
                try
                {
                    client = await _listener.AcceptTcpClientAsync();
                }
                catch (ObjectDisposedException)
                {
                    throw new Exception("The server has stopped listening or was not initialized correctly.");
                }

                if (client != null)
                {
                    // There's no await - the ReadTcpData handler is going to return immediately so that we can handle the next petition.
                    // Like a Task.Run imho.
                    //var t = ReadTcpData(client);
                    var t = Task.Run(() => ReadTcpData(client));
                }
                else
                {
                    throw new Exception("The client has disconnected or can not be retrieved.");
                }
            }
        }

        private async Task ReadTcpData(TcpClient client)
        {
            // So now the development is on handle here with the server because this is the main problem to solve:
            //  - With the socket client you can retrieve the data readed and do whatever and response to that, and read
            // again etc. But the problem with server is that if the user might wants to get the data to send one thing
            // or another, but the method to get this data is called by the method StartListening() due to it is the 
            // main server while that keeps listening to new users, because we want it to support multiple users asynchronously.
            // So, if the user calls let's say a method called GetDataReceived(), the data of this variable can be of any user
            // connection.
            // With that said, a good idea might seem to implement a Observer patter. But the problem is that the user will 
            // instantiate a server class and call a method, not implement or override a method of the observer update. It
            // will be confusing and weird to implement. So if the user has only to make petitions to this api, he will have to
            // keep checking in a while true loop if there is new data. So what is implemented is a list with data with lock access
            // for multithreading safety due to multiple connections.


            //TODO: _tcpClient is a class variable so I this it's shared between all different instances 
            _tcpClient.SetTcpClient(client);
            _tcpClient.GetStream();

            byte[] data = null;
            switch (_readerAlgorithm){
                case Reader.ReaderBufferBytes:
                    data = await Utils.TcpStreamReceiver.ReceiveBytesUntilDataAvailableAsync(_tcpClient, _readerBuffer, _tcpClient.GetNetworkStream());
                    break;
                case Reader.ReaderBytesWithDelimitators:
                    data = await Utils.TcpStreamReceiver.ReceiveBytesWithDelimitators(_tcpClient, _readerStartDelimitator, _readerEndDelimitator, _tcpClient.GetNetworkStream());
                    break;
                case Reader.ReaderBytesWithEndDelimitator:
                    data = await Utils.TcpStreamReceiver.ReceiveBytesWithEndingDelimitator(_tcpClient, _readerEndDelimitator, _tcpClient.GetNetworkStream());
                    break;
                case Reader.ReaderNumberOfBytes:
                    data = await Utils.TcpStreamReceiver.ReceiveNumberOfBytes(_tcpClient, _readerBuffer, _readerBytesToRead,_tcpClient.GetNetworkStream());
                    break;
                default:
                    //TODO: think if should throw exception
                    break;
            }

            KeyValuePair<TcpClient, byte[]> recData = new KeyValuePair<TcpClient, byte[]>(_tcpClient.GetTcpClient(), data);
            lock (_listLock)
            {
                _dataReceivedList.Add(recData);
            }

        }

        /// <inheritdoc/>
        public bool IsNewData() => (_dataReceivedList.Count > 0 ) ? true : false;

        /// <inheritdoc/>
        public KeyValuePair<TcpClient, byte[]> GetData()
        {
            KeyValuePair<TcpClient, byte[]> dataToReturn;
            lock(_listLock){
                dataToReturn = _dataReceivedList.FirstOrDefault();
                _dataReceivedList.RemoveAt(0);
            }
            return dataToReturn;
        }

        /// <inheritdoc/>
        public void CloseServer()
        {
            _listen = false;
            _cts.Cancel();
            _listener.Stop();
        }

        /// <inheritdoc/>
        public void SetReaderBytes()
        {
            _readerBuffer = 1024;
            _readerAlgorithm = Reader.ReaderBufferBytes;
        }

        /// <inheritdoc/>
        public void SetReaderBufferBytes(int bufferSize)
        {
            _readerBuffer = bufferSize;
            _readerAlgorithm = Reader.ReaderBufferBytes;        
        }

        /// <inheritdoc/>
        public void SetReaderNumberOfBytes(int bufferSize, int numberBytesToRead)
        {
            _readerBuffer = bufferSize;
            _readerBytesToRead = numberBytesToRead;
            _readerAlgorithm = Reader.ReaderNumberOfBytes;
        }

        /// <inheritdoc/>
        public void SetReaderBytesWithDelimitators(byte[] startDelimitator, byte[] endDelimitator) 
        {
            _readerStartDelimitator = startDelimitator;
            _readerEndDelimitator = endDelimitator;
            _readerAlgorithm = Reader.ReaderBytesWithDelimitators;
        }

        /// <inheritdoc/>
        public void SetReaderBytesWithEndDelimitator(byte[] endDelimitator) 
        {
            _readerEndDelimitator = endDelimitator;
            _readerAlgorithm = Reader.ReaderBytesWithEndDelimitator;
        }

        /// <inheritdoc/>
        public async Task ResponseToClient(TcpClient client, string data) => await ResponseToClient(client, Utils.Conversor.StringToBytes(data));

        /// <inheritdoc/>
        public async Task ResponseToClient(TcpClient client, byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                ITcpClient clientConnected = _tcpClient;
                clientConnected.SetTcpClient(client);
                clientConnected.GetStream();
                await Utils.TcpStreamSender.SendData(data, clientConnected, 10000);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

    }
}
