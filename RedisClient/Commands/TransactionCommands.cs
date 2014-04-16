using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class TransactionCommands
    {
        public static async Task SendDiscardCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Discard").ConfigureAwait(false);
        }

        public static async Task SendExecCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Exec").ConfigureAwait(false);
        }

        public static async Task SendMultiCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Multi").ConfigureAwait(false);
        }

        public static async Task SendUnWatchCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Unwatch").ConfigureAwait(false);
        }

        public static async Task SendWatchKeyCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Watch", key).ConfigureAwait(false);
        }

        public static async Task SendWatchKeysCommandAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null)
                return;

            await client.SendCommandAsync("Watch", keys.ToArray()).ConfigureAwait(false);
        }

    }
}
