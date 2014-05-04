using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class TransactionCommands
    {
        public static async Task SendDiscardAsync(this RedisClient client)
        {
            await client.SendAsync("Discard").ConfigureAwait(false);
        }

        public static async Task SendExecAsync(this RedisClient client)
        {
            await client.SendAsync("Exec").ConfigureAwait(false);
        }

        public static async Task SendMultiAsync(this RedisClient client)
        {
            await client.SendAsync("Multi").ConfigureAwait(false);
        }

        public static async Task SendUnWatchAsync(this RedisClient client)
        {
            await client.SendAsync("Unwatch").ConfigureAwait(false);
        }

        public static async Task SendWatchKeysAsync(this RedisClient client, params string[] keys)
        {
            await client.SendWatchKeysAsync((IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendWatchKeysAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("Watch", (keys ?? new string[]{})).ConfigureAwait(false);
        }
    }
}
