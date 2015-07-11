using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisClientFactory
    {
        public static async Task<RedisClient> CreateAsync(RedisClientConfig config)
        {
            Stream stream;
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(config.Host, config.Port).ConfigureAwait(false);
            stream = tcpClient.GetStream();

            return Create(stream);
        }

        public static RedisClient Create(Stream stream)
        {
            return new RedisClient(stream);
        }
    }
}
