using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class ConnectionCommands
    {
        public static Task SendAuthAsync(this RedisClient client, string password)
        {
            return client.SendAsync("Auth", password);
        }
        public static Task SendEchoAsync(this RedisClient client, string message)
        {
            return client.SendAsync("Echo", message);
        }
        public static Task SendPingAsync(this RedisClient client)
        {
            return client.SendAsync("Ping");
        }
        public static Task SendQuitAsync(this RedisClient client)
        {
            return client.SendAsync("Quit");
        }
        public static Task SendSelectAsync(this RedisClient client, int db)
        {
           return client.SendAsync("Select", db);
        }
    }
}
