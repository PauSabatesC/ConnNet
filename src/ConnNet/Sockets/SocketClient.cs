using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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

        public string SocketIP { get => _socketIP; set => _socketIP = value; }
        public int SocketPort { get => _socketPort; set => _socketPort = value; }
        public int ConnectionTimeout { get => _connectionTimeout; set => _connectionTimeout = value; }
        public int SendTimeout { get => _sendTimeout; set => _sendTimeout = value; }
        public int ReceiveTimeout { get => _receiveTimeout; set => _receiveTimeout = value; }
        internal ITcpClient ClientSocket { get => _clientSocket; set => _clientSocket = value; }

        /// <summary>
        /// Constructor that already sets connecion parameters for socket server connection.
        /// </summary>
        /// <param name="socketIP"></param>
        /// <param name="socketPort"></param>
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
            ClientSocket = tcpClient;
            ConnectionTimeout = 5000;
        }


        public void SetConnection(string serverIp, int serverPort) => SetConnection(serverIp, serverPort, 5000);

        public void SetConnection(string serverIp, int serverPort, int connectionTimeout)
        {
            SocketIP = serverIp;
            SocketPort = serverPort;
            ConnectionTimeout = connectionTimeout;
        }

        /*public bool Connect()
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
        }*/

        public async Task<bool> Connect()
        {
            await ClientSocket.Connect(SocketIP, SocketPort);

            if (ClientSocket.Connected())
            {
                ClientSocket.GetStream();
                return true;
            }
            else return false;
        }


        public void Disconnect()
        {
            if (_clientSocket.Connected())
            {
                _clientSocket.Dispose();
                _clientSocket.Close();
            }
        }

        public bool Send(string data)
        {
            throw new NotImplementedException();
        }

        public bool Send(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
