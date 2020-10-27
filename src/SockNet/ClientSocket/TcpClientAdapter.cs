using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SockNet.ClientSocket
{
    internal class TcpClientAdapter : ITcpClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream = null;

        public TcpClientAdapter()
        {
            _tcpClient = new TcpClient();
        }

        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            return _tcpClient.BeginConnect(host, port, requestCallback, state);
        }

        public void Close()
        {
            _tcpClient.Close();
        }

        public async Task Connect(string ip, int port)
        {
            //.ConfigureAwait(false) it's recommended so the user will not use the library like
            // Connect.Result() because it will be synchronous.
            // In UI apps is useful ConfigureAwait(false) due to SynchronizationContext with UI thread,
            // but in other scenarios it might not be so useful. For instance, with ASP.NET Core there
            // is no SynchronizationContext. So being this a general library, it might be a good option to use it.
            await _tcpClient.ConnectAsync(ip, port).ConfigureAwait(false);
        }

        public bool Connected()
        {
            return _tcpClient.Connected;
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
        }

        public void Disconnect()
        {
            _tcpClient.Client.Disconnect(true);
        }

        public void EndConnect(IAsyncResult request)
        {
            _tcpClient.EndConnect(request);
        }

        public void GetStream()
        {
            _networkStream = _tcpClient?.GetStream();
        }

        public async Task SendData(byte[] data, CancellationToken ctkn)
        {
            await _networkStream.WriteAsync(data, 0, data.Length, ctkn).ConfigureAwait(false);
            await _networkStream.FlushAsync();
        }

        public bool IsValidNetStream() => (_networkStream is null) ? false : true;

        public bool CanWrite() => _networkStream.CanWrite;

        public bool CanRead() => _networkStream.CanRead;
        public bool DataAvailable() => _networkStream.DataAvailable;

        /*public async Task<KeyValuePair<int,byte[]>> ReadData(byte[] buffer, CancellationToken ctkn)
        {
            int bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length, ctkn);
            return new KeyValuePair<int, byte[]> (bytesRead, buffer);
        }*/

        public void SetTcpClient(TcpClient client)
        {
            _tcpClient = client;
        }

        public string GetClientIP()
        {
            return ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();
        }
        public NetworkStream GetNetworkStream() => _networkStream;
        public TcpClient GetTcpClient() => _tcpClient;
    }
}
