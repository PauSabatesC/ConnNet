using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Utils
{
    internal interface IServiceLocator
    {
        T Get<T>();

    }
}
