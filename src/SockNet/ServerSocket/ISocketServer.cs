using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    public interface ISocketServer
    {
        void InitializeSocketServer(string ip, int port);

        Task StartListening();

    }
}
