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
        public static RedisClient Create(RedisClientConfig config)
        {
            return new RedisClient(new RedisSocketConnection(config));
        }
    }
}
