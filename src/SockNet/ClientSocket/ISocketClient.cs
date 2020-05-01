using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ClientSocket
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
        /// Creates a TCP client connection
        /// </summary>
        /// <returns>True if connection is created correctly.</returns>
        Task<bool> Connect();

        /// <summary>
        /// Closes the TCP socket connection correctly.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Receives an unknown number of bytes. Reads data until it stops receiving.
        /// </summary>
        /// <returns>All the bytes received.</returns>
        Task<byte[]> ReceiveBytes();

        /// <summary>
        /// Receives the message sent by the socket server.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to keep reading tcp bytes. Default is 512 bytes.</param>
        /// <returns></returns>
        Task<byte[]> ReceiveBytes(int bufferSize);

        /// <summary>
        /// Receives the message sent by the socket server.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to keep reading tcp bytes. Default is 512 bytes.</param>
        /// <param name="numberBytesToRead">Total amount of bytes you expect to receive as a final message.</param>
        /// <returns>The total amount of bytes specified received from the server.</returns>
        Task<byte[]> ReceiveNumberOfBytes(int bufferSize, int numberBytesToRead);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDelimitator"></param>
        /// <param name="endDelimitator"></param>
        /// <returns></returns>
        Task<byte[]> ReceiveBytesWithDelimitators(byte startDelimitator, byte endDelimitator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endDelimitator"></param>
        /// <returns></returns>
        Task<byte[]> ReceiveBytesWithEndDelimitator(byte endDelimitator);
    
    }
}
