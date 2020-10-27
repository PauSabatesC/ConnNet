using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SockNet.ClientSocket
{
    internal interface ITcpClient
    {
        IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        bool Connected();
        void EndConnect(IAsyncResult request);
        void Dispose();
        void Close();
        void Disconnect();
        Task Connect(string ip, int port);
        void GetStream();
        Task SendData(byte[] data, CancellationToken ctkn);
        bool IsValidNetStream();
        bool CanWrite();
        bool CanRead();
        bool DataAvailable();
        void SetTcpClient(TcpClient client);
        string GetClientIP();
        NetworkStream GetNetworkStream();
        TcpClient GetTcpClient();

    }
}
