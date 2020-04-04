using System;
using System.Threading.Tasks;

namespace ConnNet.Sockets
{
    internal interface ITcpClient
    {
        IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        bool Connected();
        void EndConnect(IAsyncResult request);
        void Dispose();
        void Close();
        Task Connect(string ip, int port);
        void GetStream();
        Task<bool> SendData(byte[] data);
        bool IsValidNetStream();

    }
}
