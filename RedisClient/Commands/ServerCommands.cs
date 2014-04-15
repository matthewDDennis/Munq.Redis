using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class ServerCommands
    {
        public static async Task SendBackgroundRewriteAOFCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("BGREWRITEAOF");
        }

        public static async Task SendBackgroundSaveCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("BGSAVE");
        }

        public static async Task SendClientKillCommandAsync(this RedisClient client, string IpAndPort)
        {
            await client.SendCommandAsync("Client", "Kill", IpAndPort);
        }

        public static async Task SendClientListCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Client", "List");
        }

        public static async Task SendClientNameCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Client", "GetName");
        }

        public static async Task SendClientPauseCommandAsync(this RedisClient client, long milliseconds)
        {
            await client.SendCommandAsync("Client", "Pause", milliseconds);
        }

        public static async Task SendClientSetNameCommandAsync(this RedisClient client, string name)
        {
            await client.SendCommandAsync("Client", "SetName", name);
        }

        public static async Task SendConfigGetCommandAsync(this RedisClient client, string parameter)
        {
            await client.SendCommandAsync("Config", "Get", parameter);
        }

        public static async Task SendConfigRewriteCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Config", "Rewrite");
        }

        public static async Task SendConfigSetCommandAsync(this RedisClient client, string parameter, object value)
        {
            await client.SendCommandAsync("Config", "Set", parameter, value);
        }

        public static async Task SendConfigResetStatCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Config", "ResetStat");
        }

        public static async Task SendDbSizeCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("DbSize");
        }

        public static async Task SendFlushAllCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("FlushAll");
        }

        public static async Task SendFlushDbCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("FlushDb");
        }

        public static async Task SendInfoCommandAsync(this RedisClient client, 
                                                      InfoSections section = InfoSections.Default)
        {
            await client.SendCommandAsync("Info", section);
        }

        public static async Task SendLastSaveCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("LastSave");
        }

        public static async Task SendMonitorCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Monitor");
        }

        public static async Task SendSaveCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Save");
        }

        public static async Task SendShutdownCommandAsync(this RedisClient client, 
                                                  ShutDownOptions option = ShutDownOptions.Default)
        {
            if (option == ShutDownOptions.Default)
                await client.SendCommandAsync("Shutdown");
            else
                await client.SendCommandAsync("ShutDown", option);
        }

        public static async Task SendSlaveOfCommandAsync(this RedisClient client, string host, int port)
        {
            await client.SendCommandAsync("SlaveOf", host, port);
        }

        public static async Task SendSlowLogGetCommandAsync(this RedisClient client, long? numToGet)
        {
            if (numToGet.HasValue)
                await client.SendCommandAsync("SlowLog", "Get", numToGet.Value);
            else
                await client.SendCommandAsync("SlowLog", "Get");
        }

        public static async Task SendSlowLogLenCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("SlowLog", "Len");
        }

        public static async Task SendSlowLogResetCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("SlowLog", "Reset");
        }

        public static async Task SendSyncCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Sync");
        }

        public static async Task SendTimeCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("Time");
        }

    }
}
