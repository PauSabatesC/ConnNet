using SockNet.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    public class SocketServer : ISocketServer
    {

        private ITcpServer _listener;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private IPAddress _ip;
        private int _port;

        internal CancellationTokenSource Cts { get => _cts; }
        internal CancellationToken Token { get => _token; }
        internal ITcpServer Listener { get => _listener; }
        public IPAddress Ip { get => _ip; }
        public int Port { get => _port; }

        /// <summary>
        /// Creates the socket server instance
        /// </summary>
        public SocketServer() : this( ServiceLocator.Current.Get<ITcpServer>()) { }

        internal SocketServer(ITcpServer listener)
        {
            _listener = listener;
        }

        public void InitializeSocketServer(string ip, int port)
        {
            _ip = Utils.Conversor.StringToIPAddress(ip);
            _port = port;

            _cts = new CancellationTokenSource();
            _token = _cts.Token;

            _listener.CreateTcpListener(_ip, _port);
        }

        public async Task StartListening()
        {
            _listener.Start();

            var client = default(TcpClient);

            while (!_token.IsCancellationRequested)
            {
                try
                {
                    await _listener.AcceptTcpClientAsync();
                }
                catch (ObjectDisposedException)
                {
                    // The listener has been stopped.
                    return;
                }

                if (client == null) return;

                // Again, there's no await - the Accept handler is going to return immediately so that we can handle the next client.
                var t = ReadTcpData(client);
            }
        }

        public void ReadTcpData(TcpClient client)
        {

        }



    }
}
