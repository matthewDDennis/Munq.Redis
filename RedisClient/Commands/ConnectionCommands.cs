using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class ConnectionCommands
    {
        public static async Task SendAuthCommandAsync(this RedisClient client, string password)
        {
            await client.SendCommandAsync("Auth", password).ConfigureAwait(false);
        }

        public static async Task SendEchoCommandAsync(this RedisClient client, string message)
        {
            await client.SendCommandAsync("Echo", message).ConfigureAwait(false);
        }

        public static async Task SendPingCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Ping").ConfigureAwait(false);
        }

        public static async Task SendQuitCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Quit").ConfigureAwait(false);
        }

        public static async Task SendSelectCommandAsync(this RedisClient client, int db)
        {
            await client.SendCommandAsync("Select", db).ConfigureAwait(false);
        }
    }
}
