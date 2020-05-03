using Ninject;
using System;
using System.Collections.Generic;
using System.Text;
using SockNet.ClientSocket;
using SockNet.ServerSocket;

namespace SockNet.Utils
{
    internal class ServiceLocator
    {
        private static IServiceLocator _serviceLocator;

        static ServiceLocator()
        {
            _serviceLocator = new DefaultServiceLocator();
        }

        public static IServiceLocator Current
        {
            get
            {
                return _serviceLocator;
            }
        }

        private sealed class DefaultServiceLocator : IServiceLocator
        {
            private readonly IKernel kernel;  // Ninject kernel

            public DefaultServiceLocator()
            {
                kernel = new StandardKernel();
                LoadBindings();
            }

            public T Get<T>()
            {
                return kernel.Get<T>();
            }

            private void LoadBindings()
            {
                kernel.Bind<ITcpClient>().To<TcpClientAdapter>().InSingletonScope();
                kernel.Bind<ITcpServer>().To<TcpListenerAdapter>().InSingletonScope();

            }
        }
    }
}
