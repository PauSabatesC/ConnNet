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

        public async Task Connect(string ip, int port, int timeout)
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
    }
}
