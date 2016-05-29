using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Munq.Redis.Tests.Requests
{
    public class CommandsWithNoParameters
    {
        static readonly Encoding encoder = new UTF8Encoding();

        public enum Command
        {
            // Connection Commands
            Ping,
            Quit,

            // Key Commands
            RandomKey,

            // Server Commands
            BGREWRITEAOF,
            BGSAVE,
            DbSize,
            FlushAll,
            FlushDb,
            Info,
            LastSave,
            Monitor,
            Save,
            Shutdown,
            Sync,
            Time,

            // Transaction Commands
            Discard,
            Exec,
            Multi,
            Unwatch,
        }

        readonly Func<RedisClient, Task>[] Methods = {
            // Connection Commands
            Redis.Requests.ConnectionCommands.SendPingAsync,
            Redis.Requests.ConnectionCommands.SendQuitAsync,

            // Key Commands
            Redis.Requests.KeyCommands.SendRandomKeyAsync,

            // Server Commands
            Redis.Requests.ServerCommands.SendBackgroundRewriteAOFAsync,
            Redis.Requests.ServerCommands.SendBackgroundSaveAsync,
            Redis.Requests.ServerCommands.SendDbSizeAsync,
            Redis.Requests.ServerCommands.SendFlushAllAsync,
            Redis.Requests.ServerCommands.SendFlushDbAsync,
            Redis.Requests.ServerCommands.SendInfoAsync,
            Redis.Requests.ServerCommands.SendLastSaveAsync,
            Redis.Requests.ServerCommands.SendMonitorAsync,
            Redis.Requests.ServerCommands.SendSaveAsync,
            Redis.Requests.ServerCommands.SendShutdownAsync,
            Redis.Requests.ServerCommands.SendSyncAsync,
            Redis.Requests.ServerCommands.SendTimeAsync,

            // Transaction Commands
            Redis.Requests.TransactionCommands.SendDiscardAsync,
            Redis.Requests.TransactionCommands.SendExecAsync,
            Redis.Requests.TransactionCommands.SendMultiAsync,
            Redis.Requests.TransactionCommands.SendUnWatchAsync
        };

        public static IEnumerable<object[]> Commands => ((int[])Enum.GetValues(typeof(Command)))
                                                         .Select(x => new object[] { (Command)x });

        [Theory, MemberData("Commands")]
        public async Task CommandOK(Command command)
        {
            var stream = new MemoryStream();
            byte[] result;
            string cmdStr = command.ToString();

            byte[] expected = encoder.GetBytes($"*1\r\n${cmdStr.Length}\r\n{cmdStr}\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Methods[(int)command](client);

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

    }
}
