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
    ///<inheritdoc/>
    public sealed class SocketClient : ISocketClient
    {
        #region private local variables declaration
        private string _socketIP;
        private int _socketPort;
        private int _connectionTimeout;
        private int _sendTimeout;
        private int _receiveTimeout;
        private ITcpClient _TcpClient;
        private byte[] _messageReaded;
        private int _bufferSize;

        #endregion

        /// <summary>
        /// IP of the server specified.
        /// </summary>
        public string SocketIP { get => _socketIP; }

        /// <summary>
        /// Port of the server specified.
        /// </summary>
        public int SocketPort { get => _socketPort; }

        /// <summary>
        /// Timeout for the initial socket connection.
        /// </summary>
        public int ConnectionTimeout { get => _connectionTimeout; }

        /// <summary>
        /// Timeout for the sending messages.
        /// </summary>
        public int SendTimeout { get => _sendTimeout; }

        /// <summary>
        /// Timeout for when receiving data.
        /// </summary>
        public int ReceiveTimeout { get => _receiveTimeout; }

        /// <summary>
        /// Instance with information of the TcpClient and NetworkStream.
        /// </summary>
        internal ITcpClient TcpClient { get => _TcpClient; }

        /// <summary>
        /// Message readed from the server.
        /// </summary>
        public byte[] MessageReaded { get => _messageReaded; }

        /// <summary>
        /// Buffer size for tcp sending or receiving data.
        /// </summary>
        public int BufferSize { get => _bufferSize; }

        /// <summary>
        /// Constructor that sets connection IP and port for socket server connection.
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Disconnect()
        {
            if (TcpClient.Connected())
            {
                TcpClient.Disconnect();
            }
        }

        /// <inheritdoc/>
        public void Close()
        {
            TcpClient.Close(); 
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            TcpClient.Dispose();
        }

        /// <inheritdoc />
        public async Task Send(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException(data, "The message to send can not be empty.");

            await Send(Utils.Conversor.StringToBytes(data), SendTimeout);
        }

        /// <inheritdoc />
        public async Task Send(byte[] data)
        {
            await Send(data, SendTimeout);
        }

        /// <inheritdoc />
        public async Task Send(byte[] data, int sendTimeout)
        {
            if (data is null) throw new ArgumentNullException();
            if (sendTimeout <= 0) throw new ArgumentException(sendTimeout.ToString(), "Timeout has to be greater than 0.");

            await Utils.TcpStreamSender.SendData(data, _TcpClient, _sendTimeout);
        }

        /// <inheritdoc />
        public async Task<byte[]> ReceiveBytes()
        {
            _messageReaded = await Utils.TcpStreamReceiver.ReceiveBytesUntilDataAvailableAsync(TcpClient, BufferSize, TcpClient.GetNetworkStream());
            return _messageReaded;
        }

        /// <inheritdoc />
        public async Task<byte[]> ReceiveBytes(int bufferSize)
        {
            _messageReaded = await Utils.TcpStreamReceiver.ReceiveBytesUntilDataAvailableAsync(TcpClient, bufferSize, TcpClient.GetNetworkStream());
            return _messageReaded;
        }

        /// <inheritdoc />
        public async Task<byte[]> ReceiveNumberOfBytes(int bufferSize, int numberBytesToRead)
        {
            _messageReaded = await Utils.TcpStreamReceiver.ReceiveNumberOfBytes(TcpClient, bufferSize, numberBytesToRead, TcpClient.GetNetworkStream());
            return _messageReaded;
        }

        /// <inheritdoc />
        public async Task<byte[]> ReceiveBytesWithDelimitators(byte[] startDelimitator, byte[] endDelimitator)
        {
            _messageReaded = await Utils.TcpStreamReceiver.ReceiveBytesWithDelimitators(TcpClient, startDelimitator, endDelimitator, TcpClient.GetNetworkStream());
            return _messageReaded;
        }

        /// <inheritdoc />
        public async Task<byte[]> ReceiveBytesWithEndDelimitator(byte[] endDelimitator)
        {
            _messageReaded = await Utils.TcpStreamReceiver.ReceiveBytesWithEndingDelimitator(TcpClient, endDelimitator, TcpClient.GetNetworkStream());
            return _messageReaded;
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
