using System.Threading.Tasks;

namespace Munq.Redis.Server
{
    public static class Commands
    {
        public static Task SendBackgroundRewriteAOFAsync(this RedisClient client)
        {
            return client.SendAsync("BGREWRITEAOF");
        }

        public static Task SendBackgroundSaveAsync(this RedisClient client)
        {
            return client.SendAsync("BGSAVE");
        }

        public static Task SendClientKillAsync(this RedisClient client, string IpAndPort)
        {
            return client.SendAsync("Client", "Kill", IpAndPort);
        }

        public static Task SendClientListAsync(this RedisClient client)
        {
            return client.SendAsync("Client", "List");
        }

        public static Task SendClientNameAsync(this RedisClient client)
        {
            return client.SendAsync("Client", "GetName");
        }

        public static Task SendClientPauseAsync(this RedisClient client, long milliseconds)
        {
            return client.SendAsync("Client", "Pause", milliseconds);
        }

        public static Task SendClientSetNameAsync(this RedisClient client, string name)
        {
            return client.SendAsync("Client", "SetName", name);
        }

        public static Task SendConfigGetAsync(this RedisClient client, string parameter)
        {
            return client.SendAsync("Config", "Get", parameter);
        }

        public static Task SendConfigResetStatAsync(this RedisClient client)
        {
            return client.SendAsync("Config", "ResetStat");
        }

        public static Task SendConfigRewriteAsync(this RedisClient client)
        {
            return client.SendAsync("Config", "Rewrite");
        }
        public static Task SendConfigSetAsync(this RedisClient client,
                                                    string parameter, object value)
        {
            return client.SendAsync("Config", "Set", parameter, value);
        }

        public static Task SendDbSizeAsync(this RedisClient client)
        {
            return client.SendAsync("DbSize");
        }
        public static Task SendFlushAllAsync(this RedisClient client)
        {
            return client.SendAsync("FlushAll");
        }

        public static Task SendFlushDbAsync(this RedisClient client)
        {
            return client.SendAsync("FlushDb");
        }

        public static Task SendInfoAsync(this RedisClient client)
        {
            return client.SendAsync("Info");
        }

        public static Task SendInfoAsync(this RedisClient client, InfoSections section)
        {
            return client.SendAsync("Info", section);
        }

        public static Task SendLastSaveAsync(this RedisClient client)
        {
            return client.SendAsync("LastSave");
        }

        public static Task SendMonitorAsync(this RedisClient client)
        {
            return client.SendAsync("Monitor");
        }

        public static Task SendSaveAsync(this RedisClient client)
        {
            return client.SendAsync("Save");
        }

        public static Task SendShutdownAsync(this RedisClient client)
        {
            return client.SendAsync("Shutdown");
        }

        public static Task SendShutdownAsync(this RedisClient client, ShutDownOptions option)
        {
            return client.SendAsync("ShutDown", option);
        }

        public static Task SendSlaveOfAsync(this RedisClient client, string host, int port)
        {
            return client.SendAsync("SlaveOf", host, port);
        }

        public static Task SendSlowLogGetAsync(this RedisClient client, long numToGet)
        {
            return client.SendAsync("SlowLog", "Get", numToGet);
        }

        public static Task SendSlowLogGetAsync(this RedisClient client)
        {
            return client.SendAsync("SlowLog", "Get");
        }

        public static Task SendSlowLogLenAsync(this RedisClient client)
        {
            return client.SendAsync("SlowLog", "Len");
        }

        public static Task SendSlowLogResetAsync(this RedisClient client)
        {
            return client.SendAsync("SlowLog", "Reset");
        }
        public static Task SendSyncAsync(this RedisClient client)
        {
            return client.SendAsync("Sync");
        }

        public static Task SendTimeAsync(this RedisClient client)
        {
            return client.SendAsync("Time");
        }
    }
}
