using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Transactions
{
    public static class Commands
    {
        public static Task SendDiscardAsync(this RedisClient client)
        {
            return client.SendAsync("Discard");
        }

        public static Task SendExecAsync(this RedisClient client)
        {
            return client.SendAsync("Exec");
        }

        public static Task SendMultiAsync(this RedisClient client)
        {
            return client.SendAsync("Multi");
        }

        public static Task SendUnWatchAsync(this RedisClient client)
        {
            return client.SendAsync("Unwatch");
        }

        public static Task SendWatchKeysAsync(this RedisClient client, params string[] keys)
        {
            return client.SendWatchKeysAsync((IEnumerable<string>) keys);
        }

        public static Task SendWatchKeysAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("Watch", keys);
        }
    }
}
