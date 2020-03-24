using System;
using System.Collections.Generic;
using System.Text;

namespace ConnNet.Sockets
{
    public interface ISocketClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Send<T>();

        /// <summary>
        /// Sets parameters to the connection to the server.
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        void SetConnection(string serverIp, int serverPort);
        
        /// <summary>
        /// Sets parameters to the connection to the server.
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="connectionTimeout">In miliseconds</param>
        void SetConnection(string serverIp, int serverPort, int connectionTimeout);


        /// <summary>
        /// Creates a TCP client connection
        /// </summary>
        /// <returns>True if connection is created correctly.</returns>
        bool Connect();

        /// <summary>
        /// Closes the TCP socket connection correctly.
        /// </summary>
        void Disconnect();
    }
}
