using SockNet.ClientSocket;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SockNet.Utils
{
    internal static class TcpStreamSender
    {
        public static async Task SendData(byte[] data, ITcpClient tcpClient, int sendTimeout)
        {
            using (var writeCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(sendTimeout)))
            {
                try
                {
                    if (tcpClient.Connected() &&
                       tcpClient.IsValidNetStream() &&
                       tcpClient.CanWrite()
                       ) await tcpClient.SendData(data, writeCts.Token);
                    else throw new Exception("Network stream to send data is not initialized or it's busy. You should create a tcp connection first with SocketClient constructor and check that no errors appear.");
                    //TODO: change exception type
                }
                catch (OperationCanceledException)
                {
                    throw new OperationCanceledException("Timeout of " + sendTimeout + " trying to send the data.");
                }
            }
        }
    }
}
