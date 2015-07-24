using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisSocketConnection : IRedisConnection
    {
        readonly RedisClientConfig _config;
        readonly TcpClient _tcpClient = new TcpClient();

        public RedisSocketConnection(RedisClientConfig config)
        {
            _config                   = config;
            _tcpClient.SendTimeout    = _config.SendTimeout;
            _tcpClient.ReceiveTimeout = _config.ReceiveTimeout;

            Database                  = _config.Database;
        }

        public bool IsConnected => _tcpClient.Connected;

        public int Database { get; }

        public void Close()
        {
#if DNXCORE50
            _tcpClient.Dispose();
#else
            _tcpClient.Close();
#endif
        }

        public async Task ConnectAsync()
        {
            if (!IsConnected)
            {
                await _tcpClient.ConnectAsync(_config.Host, _config.Port);
                if (Database != 0)
                {
                    // TODO: Select the correct Database
                }
            }
        }

        public Stream GetStream()
        {
            return _tcpClient.GetStream();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
