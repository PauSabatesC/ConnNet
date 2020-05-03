using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SockNet.ServerSocket
{
    public interface ISocketServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void InitializeSocketServer(string ip, int port);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task StartListening();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        KeyValuePair<TcpClient, byte[]> GetData();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsNewData();

        /// <summary>
        /// 
        /// </summary>
        void CloseServer();

        /// <summary>
        /// 
        /// </summary>
        void SetReaderBytes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        void SetReaderBufferBytes(int bufferSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <param name="numberBytesToRead"></param>
        //void SetReaderNumberOfBytes(int bufferSize, int numberBytesToRead);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDelimitator"></param>
        /// <param name="endDelimitator"></param>
        //void SetReaderBytesWithDelimitators(byte startDelimitator, byte endDelimitator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endDelimitator"></param>
        //void SetReaderBytesWithEndDelimitator(byte endDelimitator);

        ///
        void ResponseToClient(TcpClient client, string data);

        ///
        void ResponseToClient(TcpClient client, byte[] data);

    }
}
