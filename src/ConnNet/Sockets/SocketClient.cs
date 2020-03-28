using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ConnNet.Utils;

namespace ConnNet.Sockets
{
    public class SocketClient : ISocketClient
    {
        private string _socketIP;
        private int _socketPort;
        private int _connectionTimeout;
        private int _sendTimeout;
        private int _receiveTimeout;
        private ITcpClient _clientSocket;
        private IAsyncSocket _asyncSocket;

        public string SocketIP { get => _socketIP; set => _socketIP = value; }
        public int SocketPort { get => _socketPort; set => _socketPort = value; }
        public int ConnectionTimeout { get => _connectionTimeout; set => _connectionTimeout = value; }
        public int SendTimeout { get => _sendTimeout; set => _sendTimeout = value; }
        public int ReceiveTimeout { get => _receiveTimeout; set => _receiveTimeout = value; }
        internal ITcpClient ClientSocket { get => _clientSocket; set => _clientSocket = value; }
        internal IAsyncSocket AsyncSocket { get => _asyncSocket; set => _asyncSocket = value; }

        /// <summary>
        /// Constructor that already sets connecion parameters for socket server connection.
        /// </summary>
        /// <param name="socketIP"></param>
        /// <param name="socketPort"></param>
        public SocketClient(string socketIP, int socketPort) 
            :this(
                    socketIP, 
                    socketPort, 
                    ServiceLocator.Current.Get<ITcpClient>(), 
                    ServiceLocator.Current.Get<IAsyncSocket>()
                 )
        { }

        internal SocketClient(string socketIP, int socketPort, ITcpClient tcpClient, IAsyncSocket asyncSocket)
        {
            SocketIP = socketIP;
            SocketPort = socketPort;
            ClientSocket = tcpClient;
            AsyncSocket = asyncSocket;
            ConnectionTimeout = 5000;
        }

        public T Send<T>()
        {
            throw new NotImplementedException();
        }

        public void SetConnection(string serverIp, int serverPort) => SetConnection(serverIp, serverPort, 5000);

        public void SetConnection(string serverIp, int serverPort, int connectionTimeout)
        {
            SocketIP = serverIp;
            SocketPort = serverPort;
            ConnectionTimeout = connectionTimeout;
        }

        //TODO: maybe some retries can be added
        public bool Connect()
        {
            bool result = false;

            IAsyncResult request = _clientSocket.BeginConnect(_socketIP, _socketPort, null, null);
            bool success = _asyncSocket.AsyncWaitHandle(request,ConnectionTimeout);
            if (_clientSocket.Connected())
            {
                _clientSocket.EndConnect(request);
                result = true;
            }
            else
            {
                _clientSocket.Close();
            }

            return result;
        }


        public void Disconnect()
        {
            if (_clientSocket.Connected())
            {
                _clientSocket.Dispose();
                _clientSocket.Close();
            }
        }


    }
}
