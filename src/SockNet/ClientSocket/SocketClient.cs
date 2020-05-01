using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SockNet.Utils;

[assembly: InternalsVisibleTo("SockNet.UnitaryTests")]
[assembly: InternalsVisibleTo("SockNet.IntegrationTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace SockNet.ClientSocket
{

    public sealed class SocketClient : ISocketClient
    {
        private string _socketIP;
        private int _socketPort;
        private int _connectionTimeout;
        private int _sendTimeout;
        private int _receiveTimeout;
        private ITcpClient _TcpClient;
        private byte[] _messageReaded;
        private int _bufferSize;

        public string SocketIP { get => _socketIP; }
        public int SocketPort { get => _socketPort; }
        public int ConnectionTimeout { get => _connectionTimeout; }
        public int SendTimeout { get => _sendTimeout; }
        public int ReceiveTimeout { get => _receiveTimeout; }
        internal ITcpClient TcpClient { get => _TcpClient; }
        public byte[] MessageReaded { get => _messageReaded; }
        public int BufferSize { get => _bufferSize; }

        /// <summary>
        /// Constructor that sets connecion ip and port dor socket server connection.
        /// </summary>
        /// <param name="socketIP">IP represented as string.</param>
        /// <param name="socketPort">Port number of the server listening.</param>
        public SocketClient(string socketIP, int socketPort)
            :this(
                    socketIP, 
                    socketPort, 
                    ServiceLocator.Current.Get<ITcpClient>()
                 )
        { }

        internal SocketClient(string socketIP, int socketPort, ITcpClient tcpClient)
        {
            _socketIP = socketIP;
            _socketPort = socketPort;
            _TcpClient = tcpClient;
            _connectionTimeout = 5000;
            _sendTimeout = 10000;
            _receiveTimeout = 10000;
            _bufferSize = 512;
            
        }

        public async Task<bool> Connect()
        {
            bool success = false;
            await TcpClient.Connect(SocketIP, SocketPort);

            if (TcpClient.Connected())
            {
                TcpClient.GetStream();
                if (TcpClient.IsValidNetStream()) success = true;
                else success = false;
            }
            else success = false;

            return success;
        }


        public void Disconnect()
        {
            if (TcpClient.Connected())
            {
                TcpClient.Dispose();
                TcpClient.Close();
            }
        }

        public async Task Send(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException(data, "The message to send can not be empty.");

            await Send(Utils.Conversor.StringToBytes(data), SendTimeout);
        }

        public async Task Send(byte[] data)
        {
            await Send(data, SendTimeout);
        }

        public async Task Send(byte[] data, int sendTimeout)
        {
            if (data is null) throw new ArgumentNullException();
            if (sendTimeout <= 0) throw new ArgumentException(sendTimeout.ToString(), "Timeout has to be greater than 0.");
            
            using (var writeCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(sendTimeout)))
            {
                try
                {
                    if(TcpClient.Connected() &&
                       TcpClient.IsValidNetStream() && 
                       TcpClient.CanWrite()
                       ) await TcpClient.SendData(data, writeCts.Token);
                    else throw new Exception("Network stream to send data is not initialized or it's busy. You should create a tcp connection first with SocketClient constructor and check that no errors appear.");
                    //TODO: change exception type
                }
                catch(OperationCanceledException)
                {
                    throw new OperationCanceledException("Timeout of " + sendTimeout + " trying to send the data.");
                }
            }
        }

        public async Task<byte[]> ReceiveBytes()
        {
            _messageReaded = await Utils.TcpStreamReceiver.ReceiveBytesUntilDataAvailableAsync(TcpClient, BufferSize);
            return _messageReaded;
        }

        public Task<byte[]> ReceiveBytes(int bufferSize)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReceiveNumberOfBytes(int bufferSize, int numberBytesToRead)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReceiveBytesWithDelimitators(byte startDelimitator, byte endDelimitator)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> ReceiveBytesWithEndDelimitator(byte endDelimitator)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the message received in bytes to ASCII string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Encoding.ASCII.GetString(_messageReaded);
        }
    }
}
