using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ClientSocket
{
    /// <summary>
    /// API for creating the socket in the client side.
    /// </summary>
    public interface ISocketClient
    {
        /// <summary>
        /// Sends a string of data specified to the socket server.
        /// </summary>
        /// <returns></returns>
        Task Send(string data);

        /// <summary>
        /// Sends as an array of bytes specified to the socket server.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Send(byte[] data);

        /// <summary>
        /// Creates a TCP client connection to the server.
        /// </summary>
        /// <returns>True if connection is created correctly.</returns>
        Task<bool> Connect();

        /// <summary>
        /// Closes the socket connection and allows its reuse.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Closes and disposes the client tcp connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Free all assigned resources to socket client. Not necessary to call if calling Close().
        /// </summary>
        void Dispose();
    
        /// <summary>
        /// Receives an unknown number of bytes. Reads data until it stops receiving.
        /// </summary>
        /// <returns>All the bytes received.</returns>
        Task<byte[]> ReceiveBytes();

        /// <summary>
        /// Receives an unknown number of bytes. Reads data until it stops receiving.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to keep reading tcp bytes. Default is 512 bytes.</param>
        /// <returns></returns>
        Task<byte[]> ReceiveBytes(int bufferSize);

        /// <summary>
        /// Receives the specified number of bytes.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to keep reading tcp bytes. Default is 512 bytes.</param>
        /// <param name="numberBytesToRead">Total amount of bytes you expect to receive as a final message.</param>
        /// <returns>The total amount of bytes specified received from the server.</returns>
        Task<byte[]> ReceiveNumberOfBytes(int bufferSize, int numberBytesToRead);

        /// <summary>
        /// Receives the tcp data telegram. Reads from the start delimitator until the end delimitator is reached.
        /// </summary>
        /// <param name="startDelimitator">The fixed sequence of bytes that indicates the start of the telegram.</param>
        /// <param name="endDelimitator">The fixed sequence of bytes that indicates the end of the telegram.</param>
        /// <returns></returns>
        Task<byte[]> ReceiveBytesWithDelimitators(byte[] startDelimitator, byte[] endDelimitator);
        //TODO: change the method with a timeout in the parameters in case no end delimitator is received

        /// <summary>
        /// Receives the tcp data telegram. Reads since something is received until the end delimitator is reached.
        /// </summary>
        /// <param name="endDelimitator">The fixed sequence of bytes that indicates the end of the telegram.</param>
        /// <returns></returns>
        Task<byte[]> ReceiveBytesWithEndDelimitator(byte[] endDelimitator);
    
    }
}
