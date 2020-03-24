using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Sockets
{
    public interface ITcpClient
    {
        IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        bool Connected();
        void EndConnect(IAsyncResult request);
        void Dispose();
        void Close();
    }
}
