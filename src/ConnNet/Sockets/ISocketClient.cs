using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnNet.Sockets
{
    public interface ISocketClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Send(string data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Send(byte[] data);

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
        Task<bool> Connect();

        /// <summary>
        /// Closes the TCP socket connection correctly.
        /// </summary>
        void Disconnect();
    }
}
