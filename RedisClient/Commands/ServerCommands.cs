using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class ServerCommands
    {
        public static async Task SendBackgroundRewriteAOFAsync(this RedisClient client)
        {
            await client.SendAsync("BGREWRITEAOF").ConfigureAwait(false);
        }
        public static async Task SendBackgroundSaveAsync(this RedisClient client)
        {
            await client.SendAsync("BGSAVE").ConfigureAwait(false);
        }
        public static async Task SendClientKillAsync(this RedisClient client, string IpAndPort)
        {
            await client.SendAsync("Client", "Kill", IpAndPort).ConfigureAwait(false);
        }
        public static async Task SendClientListAsync(this RedisClient client)
        {
            await client.SendAsync("Client", "List").ConfigureAwait(false);
        }
        public static async Task SendClientNameAsync(this RedisClient client)
        {
            await client.SendAsync("Client", "GetName").ConfigureAwait(false);
        }
        public static async Task SendClientPauseAsync(this RedisClient client, long milliseconds)
        {
            await client.SendAsync("Client", "Pause", milliseconds).ConfigureAwait(false);
        }
        public static async Task SendClientSetNameAsync(this RedisClient client, string name)
        {
            await client.SendAsync("Client", "SetName", name).ConfigureAwait(false);
        }
        public static async Task SendConfigGetAsync(this RedisClient client, string parameter)
        {
            await client.SendAsync("Config", "Get", parameter).ConfigureAwait(false);
        }
        public static async Task SendConfigResetStatAsync(this RedisClient client)
        {
            await client.SendAsync("Config", "ResetStat").ConfigureAwait(false);
        }
        public static async Task SendConfigRewriteAsync(this RedisClient client)
        {
            await client.SendAsync("Config", "Rewrite").ConfigureAwait(false);
        }
        public static async Task SendConfigSetAsync(this RedisClient client,
                                                    string parameter, object value)
        {
            await client.SendAsync("Config", "Set", parameter, value).ConfigureAwait(false);
        }
        public static async Task SendDbSizeAsync(this RedisClient client)
        {
            await client.SendAsync("DbSize").ConfigureAwait(false);
        }
        public static async Task SendFlushAllAsync(this RedisClient client)
        {
            await client.SendAsync("FlushAll").ConfigureAwait(false);
        }
        public static async Task SendFlushDbAsync(this RedisClient client)
        {
            await client.SendAsync("FlushDb").ConfigureAwait(false);
        }
        public static async Task SendInfoAsync(this RedisClient client)
        {
            await client.SendAsync("Info").ConfigureAwait(false);
        }
        public static async Task SendInfoAsync(this RedisClient client, InfoSections section)
        {
            await client.SendAsync("Info", section).ConfigureAwait(false);
        }
        public static async Task SendLastSaveAsync(this RedisClient client)
        {
            await client.SendAsync("LastSave").ConfigureAwait(false);
        }
        public static async Task SendMonitorAsync(this RedisClient client)
        {
            await client.SendAsync("Monitor").ConfigureAwait(false);
        }
        public static async Task SendSaveAsync(this RedisClient client)
        {
            await client.SendAsync("Save").ConfigureAwait(false);
        }
        public static async Task SendShutdownAsync(this RedisClient client)
        {
            await client.SendAsync("Shutdown").ConfigureAwait(false);
        }
        public static async Task SendShutdownAsync(this RedisClient client, ShutDownOptions option)
        {
            await client.SendAsync("ShutDown", option).ConfigureAwait(false);
        }
        public static async Task SendSlaveOfAsync(this RedisClient client, string host, int port)
        {
            await client.SendAsync("SlaveOf", host, port).ConfigureAwait(false);
        }
        public static async Task SendSlowLogGetAsync(this RedisClient client, long numToGet)
        {
            await client.SendAsync("SlowLog", "Get", numToGet).ConfigureAwait(false);
        }
        public static async Task SendSlowLogGetAsync(this RedisClient client)
        {
            await client.SendAsync("SlowLog", "Get").ConfigureAwait(false);
        }
        public static async Task SendSlowLogLenAsync(this RedisClient client)
        {
            await client.SendAsync("SlowLog", "Len").ConfigureAwait(false);
        }
        public static async Task SendSlowLogResetAsync(this RedisClient client)
        {
            await client.SendAsync("SlowLog", "Reset").ConfigureAwait(false);
        }
        public static async Task SendSyncAsync(this RedisClient client)
        {
            await client.SendAsync("Sync").ConfigureAwait(false);
        }
        public static async Task SendTimeAsync(this RedisClient client)
        {
            await client.SendAsync("Time").ConfigureAwait(false);
        }
    }
}
