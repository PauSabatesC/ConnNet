using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

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

        public string SocketIP { get => _socketIP; set => _socketIP = value; }
        public int SocketPort { get => _socketPort; set => _socketPort = value; }
        public int ConnectionTimeout { get => _connectionTimeout; set => _connectionTimeout = value; }
        public int SendTimeout { get => _sendTimeout; set => _sendTimeout = value; }
        public int ReceiveTimeout { get => _receiveTimeout; set => _receiveTimeout = value; }
        public ITcpClient ClientSocket { get => _clientSocket; set => _clientSocket = value; }
        
        /// <summary>
        /// Constructor that already sets connecion parameters for socket server connection.
        /// </summary>
        /// <param name="socketIP"></param>
        /// <param name="socketPort"></param>
        public SocketClient(string socketIP, int socketPort) :this(socketIP, socketPort, new TcpClientAdapter())
        { }

        internal SocketClient(string socketIP, int socketPort, ITcpClient tcpClient)
        {
            SocketIP = socketIP;
            SocketPort = socketPort;
            ClientSocket = tcpClient;
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

        public bool Connect()
        {
             bool result = false;
             _clientSocket = new TcpClientAdapter();

             var request = _clientSocket.BeginConnect(_socketIP, _socketPort, null, null);
             bool success = request.AsyncWaitHandle.WaitOne(ConnectionTimeout, true);
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
