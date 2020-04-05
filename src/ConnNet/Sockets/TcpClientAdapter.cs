using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ConnNet.Sockets
{
    internal class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient _tcpClient;
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
            //await Task.WhenAll(_networkStream.WriteAsync(BitConverter.GetBytes(data.Length), 0, 4), _networkStream.WriteAsync(data, 0, data.Length)).ConfigureAwait(false);
            await _networkStream.WriteAsync(data, 0, data.Length, ctkn).ConfigureAwait(false);
        }

        public bool IsValidNetStream() => (_networkStream is null) ? false : true;

        public bool CanWrite() => _networkStream.CanWrite;
    }
}
