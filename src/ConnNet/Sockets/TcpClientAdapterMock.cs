using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Sockets
{
    public class TcpClientAdapterMock : ITcpClient
    {
        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            throw new NotImplementedException();
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
