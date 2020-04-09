using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConnNet.Sockets
{
    public interface ISocketClient
    {
        /// <summary>
        /// Sends as an array ogf bytes the string of data specified to the socket server.
        /// </summary>
        /// <returns></returns>
        Task Send(string data);

        /// <summary>
        /// Sends as an array ogf bytes the data specified to the socket server.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Send(byte[] data);
        
        /// <summary>
        /// Sets parameters to the connection to the server.
        /// </summary>
        /// <param name="connectionTimeout">In miliseconds</param>
        void SetConnectionOptions(int connectionTimeout);

        /// <summary>
        /// Creates a TCP client connection
        /// </summary>
        /// <returns>True if connection is created correctly.</returns>
        Task<bool> Connect();

        /// <summary>
        /// Closes the TCP socket connection correctly.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Receives the message sent by the socket server.
        /// </summary>
        //T Receive<T>(); //TODO: should analyze if I return a generic type
        Task<byte[]> ReceiveBytes();
        Task<string> ReceiveString();
    }
}
