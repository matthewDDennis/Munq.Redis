using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisSocketConnection : IRedisConnection
    {
        readonly RedisClientConfig _config;
        private TcpClient _tcpClient = new TcpClient();
        private NetworkStream _stream = null;

        public RedisSocketConnection(RedisClientConfig config)
        {
            _config                   = config;
            _tcpClient.SendTimeout    = _config.SendTimeout;
            _tcpClient.ReceiveTimeout = _config.ReceiveTimeout;

            Database                  = _config.Database;
        }

        public bool IsConnected => _tcpClient.Connected;

        public int Database { get; internal set; }

        public void Close()
        {
#if DNXCORE50
            _tcpClient.Dispose();
            if (_stream != null)
                _stream.Dispose();
#else
            _tcpClient.Close();
            if (_stream != null)
                _stream.Close();
#endif
            _stream = null;
        }

        public async Task ConnectAsync()
        {
            if (!IsConnected)
            {
                await _tcpClient.ConnectAsync(_config.Host, _config.Port).ConfigureAwait(false);
                _stream = _tcpClient.GetStream();
                Database = 0;
            }
        }

        public Task ReconnectAsync()
        {
            Close();
            _tcpClient = new TcpClient();
            return ConnectAsync();

        }

        public Stream GetStream()
        {
            return _stream;
        }

        public void Dispose()
        {
            Close();
        }

    }
}
