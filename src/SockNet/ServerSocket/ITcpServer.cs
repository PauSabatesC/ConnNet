using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    public interface ITcpServer
    {
        void CreateTcpListener(IPAddress ip, int port);

        void Start();
        Task AcceptTcpClientAsync();

    }
}
