using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    internal interface ITcpServer
    {
        void CreateTcpListener(IPAddress ip, int port);
        void Start();
        void Stop();
        Task<TcpClient> AcceptTcpClientAsync();

    }
}
