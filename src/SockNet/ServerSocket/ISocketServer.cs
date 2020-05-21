using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    /// <summary>
    /// API for creating a listener socket in the server side.
    /// </summary>
    public interface ISocketServer
    {
        /// <summary>
        /// Sets the IP and port of the socket server.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void InitializeSocketServer(string ip, int port);

        /// <summary>
        /// Starts accepting client connections asynchronously.
        /// </summary>
        /// <returns></returns>
        Task StartListening();

        /// <summary>
        /// Data obtained form a client connection.
        /// </summary>
        /// <returns>A keyValue with the client tcp information and the array of data received.</returns>
        KeyValuePair<TcpClient, byte[]> GetData();

        /// <summary>
        /// Checks if a new message has been received from some socket client.
        /// </summary>
        /// <returns></returns>
        bool IsNewData();

        /// <summary>
        /// Dispose the current socket server lintener.
        /// </summary>
        void CloseServer();

        /// <summary>
        /// Sets the server to receive an unknown number of bytes. Reads data until it stops receiving.
        /// </summary>
        void SetReaderBytes();

        /// <summary>
        /// Sets the server to receive an unknown number of bytes. Reads data until it stops receiving.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to keep reading tcp bytes. Default is 512 bytes.</param>
        void SetReaderBufferBytes(int bufferSize);

        /// <summary>
        /// Sends data to the connected socket client.
        /// </summary>
        /// <param name="client">Client connected to the server that receives the data.</param>
        /// <param name="data">Data to send.</param>
        /// <returns></returns>
        Task ResponseToClient(TcpClient client, string data);

        /// <summary>
        /// Sends data to the connected socket client.
        /// </summary>
        /// <param name="client">Client connected to the server that receives the data.</param>
        /// <param name="data">Data to send.</param>
        /// <returns></returns>
        Task ResponseToClient(TcpClient client, byte[] data);

        /// <summary>
        /// Sets the server to receive the specified number of bytes.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to keep reading tcp bytes. Default is 512 bytes.</param>
        /// <param name="numberBytesToRead">Total amount of bytes you expect to receive as a final message.</param>
        void SetReaderNumberOfBytes(int bufferSize, int numberBytesToRead);

        /// <summary>
        /// Sets the receiver to get the tcp data telegram. Reads from the start delimitator until the end delimitator is reached.
        /// </summary>
        /// <param name="startDelimitator">The fixed sequence of bytes that indicates the start of the telegram.</param>
        /// <param name="endDelimitator">The fixed sequence of bytes that indicates the end of the telegram.</param>
        void SetReaderBytesWithDelimitators(byte[] startDelimitator, byte[] endDelimitator);

        /// <summary>
        /// Sets the receiver to get the tcp data telegram. Reads since something is received until the end delimitator is reached.
        /// </summary>
        /// <param name="endDelimitator">The fixed sequence of bytes that indicates the end of the telegram.</param>
        void SetReaderBytesWithEndDelimitator(byte[] endDelimitator);

    }
}
