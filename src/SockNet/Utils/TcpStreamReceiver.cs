using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SockNet.ClientSocket;

namespace SockNet.Utils
{
    internal static class TcpStreamReceiver
    {
        private static List<byte> _incomingData;
        internal static List<byte> IncomingData { get => _incomingData; set => _incomingData = value; }

        public static async Task<byte[]> ReceiveBytesUntilDataAvailableAsync(ITcpClient TcpClient, int bufferSize, NetworkStream stream)
        {
            if (TcpClient.CanRead())
            {
                IncomingData = new List<byte>();
                byte[] buffer = new byte[bufferSize];
                int bytesRead;
                int auxBuffer = bufferSize;

                //TODO: add the cancellation token
                //using (var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                
                    while ((auxBuffer == bufferSize) &&(bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                    {
                        auxBuffer = bytesRead;
                        byte[] tempData = new byte[bytesRead];
                        Array.Copy(buffer, 0, tempData, 0, bytesRead);
                        IncomingData.AddRange(tempData);
                    }
                

                return IncomingData.ToArray();

            }
            else throw new Exception("The socket client could not start reading. Check if the server allows it or the socket client has initialized correctly.");
            //TODO: change exception type
        }


        public static async Task<byte[]> ReceiveNumberOfBytes(ITcpClient TcpClient, int bufferSize, int numberBytesToRead, NetworkStream stream) 
        {
            if (TcpClient.CanRead())
            {
                IncomingData = new List<byte>();
                byte[] buffer = new byte[bufferSize];
                int bytesRead;
                int auxBuffer=0;
                bool allBytesReceived = false;

                //TODO: add the cancellation token
                //using (var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))

                while (!allBytesReceived && (bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                {
                    if (bytesRead>=numberBytesToRead)
                    {
                        //auxBuffer = numberBytesToRead;
                        allBytesReceived = true;
                        byte[] tempDataOverflowed = new byte[numberBytesToRead];
                        Array.Copy(buffer, 0, tempDataOverflowed, 0, numberBytesToRead);
                        IncomingData.AddRange(tempDataOverflowed);
                    }
                    else
                    {
                        auxBuffer += bytesRead;
                        byte[] tempData = new byte[bytesRead];
                        Array.Copy(buffer, 0, tempData, 0, bytesRead);
                        IncomingData.AddRange(tempData);
                    }

                    if(auxBuffer==numberBytesToRead)
                    {
                        allBytesReceived = true;
                    }

                }


                return IncomingData.ToArray();

            }
            else throw new Exception("The socket client could not start reading. Check if the server allows it or the socket client has initialized correctly.");
            //TODO: change exception type
        }


        public static async Task<byte[]> ReceiveBytesWithDelimitators(ITcpClient TcpClient, byte[] startDelimitator, byte[] endDelimitator, NetworkStream stream) 
        {
            if (TcpClient.CanRead())
            {
                byte[] buffer = new byte[512];
                int bytesRead;
                bool firstChar = false;
                bool lastChar = false;
                string dataAcumulated = string.Empty;

                var strStart = Encoding.ASCII.GetString(startDelimitator);
                var strEnd = Encoding.ASCII.GetString(endDelimitator);

                //TODO: add the cancellation token
                //using (var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))

                while ((!firstChar || !lastChar) && (bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                {
                    var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    if (!firstChar && data.StartsWith(strStart))
                    {
                        firstChar = true;
                    }
                    if(firstChar)
                    {
                        //dataAcumulated += data;
                        if (data.Contains(strEnd))
                        {
                            var cleanData = data.Split(new string[] { strEnd }, StringSplitOptions.None)[0];
                            dataAcumulated = cleanData;
                            lastChar = true;
                        }
                        else
                        {
                            dataAcumulated += data;
                        }
                    }
                }

                return Utils.Conversor.StringToBytes(dataAcumulated);
            }
            else throw new Exception("The socket client could not start reading. Check if the server allows it or the socket client has initialized correctly.");
            //TODO: change exception type
        }


        public static async Task<byte[]> ReceiveBytesWithEndingDelimitator(ITcpClient TcpClient, byte[] endDelimitator, NetworkStream stream) 
        {
            if (TcpClient.CanRead())
            {
                byte[] buffer = new byte[512];
                int bytesRead;
                bool lastChar = false;
                string dataAcumulated = string.Empty;

                var strEnd = Encoding.ASCII.GetString(endDelimitator);

                //TODO: add the cancellation token
                //using (var readCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))

                while (!lastChar && (bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                {
                    var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);


                    if (data.Contains(strEnd))
                    {
                        var cleanData = data.Split(new string[] { strEnd }, StringSplitOptions.None)[0];
                        dataAcumulated += cleanData;
                        lastChar = true;
                    }
                    else
                    {
                        dataAcumulated += data;
                    }
                    
                }

                return Utils.Conversor.StringToBytes(dataAcumulated);
            }
            else throw new Exception("The socket client could not start reading. Check if the server allows it or the socket client has initialized correctly.");
            //TODO: change exception type

        }



    }
}
