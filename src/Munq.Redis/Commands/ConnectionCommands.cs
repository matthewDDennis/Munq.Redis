using System;
using System.Threading.Tasks;

namespace Munq.Redis.Connection
{
    public static class Commands
    {
        public static Task SendAuthAsync(this RedisClient client, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            return client.SendAsync("Auth", password);
        }

        public static Task SendEchoAsync(this RedisClient client, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

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
