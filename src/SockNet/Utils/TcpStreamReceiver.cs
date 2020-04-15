using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SockNet.ClientSocket;

namespace SockNet.Utils
{
    internal static class TcpStreamReceiver
    {
        private static List<byte> _incomingData;
        internal static List<byte> IncomingData { get => _incomingData; set => _incomingData = value; }

        public static async Task<byte[]> ReceiveBytesUntilDataAvailableAsync(ITcpClient TcpClient, int bufferSize)
        {
            if (TcpClient.CanRead())
            {
                IncomingData = new List<byte>();
                byte[] buffer = new byte[512];
                int bytesRead = 0;
                var infoRead = new KeyValuePair<int, byte[]>();

                while (TcpClient.DataAvailable())
                {
                    using (var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                        infoRead = await TcpClient.ReadData(buffer, readCts.Token);
                    bytesRead = infoRead.Key;
                    buffer = infoRead.Value;

                    byte[] tempData = new byte[bytesRead];
                    Array.Copy(buffer, 0, tempData, 0, bytesRead);
                    IncomingData.AddRange(tempData);

                }
                return IncomingData.ToArray();
            }
            else throw new Exception("The socket client could not start reading. Check if the server allows it or the socket client has initialized correctly.");
            //TODO: change exception type
        }


        public static async Task<byte[]> ReceiveNUmberOfBytes(ITcpClient TcpClient, int bufferSize, int numberBytesToRead)
        {
            throw new NotImplementedException();
        }

        public static async Task<byte[]> ReceiveBytesWithDelimitators(ITcpClient TcpClient, byte startDelimitator, byte endDelimitator)
        {
            throw new NotImplementedException();
        }

        public static async Task<byte[]> ReceiveBytesWithEndingDelimitator(ITcpClient TcpClient, byte endDelimitator)
        {
            throw new NotImplementedException();
        }



    }
}
