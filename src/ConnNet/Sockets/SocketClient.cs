using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
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
        }

        public void SetConnectionOptions(int connectionTimeout)
        {
            ConnectionTimeout = connectionTimeout;
        }

        public async Task<bool> Connect()
        {
            bool success = false;
            await TcpClient.Connect(SocketIP, SocketPort); //TODO: can I add connection timeout and other options?

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

        public async Task<bool> Send(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException(data, "Message to send can not be empty.");

            return await Send(Utils.Conversor.StringToBytes(data)) ? true : false;
        }

        public async Task<bool> Send(byte[] data) //TODO: I also want to specify a timeout in the send petition
        {
            if (data is null) throw new ArgumentNullException();

            return await TcpClient.SendData(data) ? true : false;
        }
    }
}
