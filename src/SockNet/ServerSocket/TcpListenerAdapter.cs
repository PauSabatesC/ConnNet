using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SockNet.ClientSocket;

namespace SockNet.ServerSocket
{
    internal class TcpListenerAdapter : ITcpServer
    {
        private TcpListener _tcpListener;
        //private TcpClient _tcpClient;
        //private NetworkStream _tcpStream;

        public void CreateTcpListener(IPAddress ip, int port)
        {
            _tcpListener = new TcpListener(ip, port);           
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public async Task<TcpClient> AcceptTcpClientAsync()
        {
            var res = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
            return res;
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }
    
    }
}
