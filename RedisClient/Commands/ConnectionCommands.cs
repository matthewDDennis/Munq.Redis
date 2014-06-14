using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class ConnectionCommands
    {
        public static async Task SendAuthAsync(this RedisClient client, string password)
        {
            await client.SendAsync("Auth", password).ConfigureAwait(false);
        }
        public static async Task SendEchoAsync(this RedisClient client, string message)
        {
            await client.SendAsync("Echo", message).ConfigureAwait(false);
        }
        public static async Task SendPingAsync(this RedisClient client)
        {
            await client.SendAsync("Ping").ConfigureAwait(false);
        }
        public static async Task SendQuitAsync(this RedisClient client)
        {
            await client.SendAsync("Quit").ConfigureAwait(false);
        }
        public static async Task SendSelectAsync(this RedisClient client, int db)
        {
            await client.SendAsync("Select", db).ConfigureAwait(false);
        }
    }
}
