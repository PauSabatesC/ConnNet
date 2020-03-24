using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ConnNet.Sockets
{
    public class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient _tcpClient;

        public TcpClientAdapter()
        {
            _tcpClient = new TcpClient();

        }
        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            return _tcpClient.BeginConnect(host, port, requestCallback, state);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Connected()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void EndConnect(IAsyncResult request)
        {
            throw new NotImplementedException();
        }
    }
}
