using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConnNet.Utils;

[assembly: InternalsVisibleTo("ConnNet.UnitaryTests")]
[assembly: InternalsVisibleTo("ConnNet.IntegrationTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ConnNet.Sockets
{

    public sealed class SocketClient : ISocketClient
    {
        private string _socketIP;
        private int _socketPort;
        private int _connectionTimeout;
        private int _sendTimeout;
        private int _receiveTimeout;
        private ITcpClient _TcpClient;

        public string SocketIP { get => _socketIP; set => _socketIP = value; }
        public int SocketPort { get => _socketPort; set => _socketPort = value; }
        public int ConnectionTimeout { get => _connectionTimeout; set => _connectionTimeout = value; }
        public int SendTimeout { get => _sendTimeout; set => _sendTimeout = value; }
        public int ReceiveTimeout { get => _receiveTimeout; set => _receiveTimeout = value; }
        internal ITcpClient TcpClient { get => _TcpClient; set => _TcpClient = value; }

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
            SocketIP = socketIP;
            SocketPort = socketPort;
            TcpClient = tcpClient;
            ConnectionTimeout = 5000;
            SendTimeout = 10000;
            ReceiveTimeout = 10000;
        }

        public void SetConnectionOptions(int connectionTimeout)
        {
            ConnectionTimeout = connectionTimeout;
        }

        public async Task<bool> Connect()
        {
            bool success = false;
            await TcpClient.Connect(SocketIP, SocketPort); //TODO: add connection timeout and other options

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
            if (_TcpClient.Connected())
            {
                _TcpClient.Dispose();
                _TcpClient.Close();
            }
        }

        public async Task Send(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException(data, "Message to send can not be empty.");

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

        public T Receive<T>()
        {
            throw new NotImplementedException();
        }
    }
}
