using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Sockets
{
    internal class AsyncSocket : IAsyncSocket
    {
        public bool AsyncWaitHandle(IAsyncResult request, int connectionTimeout)
        {
            return request.AsyncWaitHandle.WaitOne(connectionTimeout, true);
        }
    }
}
