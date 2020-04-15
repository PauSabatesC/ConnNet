using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SockNet.ClientSocket;

namespace SockNet.ServerSocket
{
    class TcpListenerAdapter : ITcpServer
    {
        private TcpListener _tcpListener;
        private TcpClient _tcpClient;

        public void CreateTcpListener(IPAddress ip, int port)
        {
            _tcpListener = new TcpListener(ip, port);
            
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public async Task AcceptTcpClientAsync()
        {
            var res = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
            _tcpClient = res;
        }

    }
}
