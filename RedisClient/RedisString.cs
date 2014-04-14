using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisString
    {
        public RedisString(RedisClient client, string key)
        {
            Client = client;
            Key = key;
        }

        public RedisClient Client { get; private set; }
        public string Key { get; private set; }
    }
}
