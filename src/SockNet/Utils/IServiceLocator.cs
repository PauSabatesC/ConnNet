using System;
using System.Collections.Generic;
using System.Text;

namespace SockNet.Utils
{
    internal interface IServiceLocator
    {
        T Get<T>();

    }
}
