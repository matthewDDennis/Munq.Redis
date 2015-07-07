using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis
{
    public class RedisClientConfig
    {
        public const string DefaultHost    = "localhost";
        public const int    DefaultPort    = 6379;
        public const int    DefaultTimeout = 1000;

        public string Host           { get; set; }
        public int    Port           { get; set; }
        public int    ReceiveTimeout { get; set; }
        public int    SendTimeout    { get; set; }

        public RedisClientConfig()
        {
            Host           = DefaultHost;
            Port           = DefaultPort;
            ReceiveTimeout = DefaultTimeout;
            SendTimeout    = DefaultTimeout;
        }
    }
}
