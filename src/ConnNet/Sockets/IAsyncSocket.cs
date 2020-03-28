using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Sockets
{
    internal interface IAsyncSocket
    {
        bool AsyncWaitHandle(IAsyncResult request, int connectionTimeout);
    }
}
