using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Munq.Redis.Tests.Commands
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
            Redis.Commands.ConnectionCommands.SendPingAsync,
            Redis.Commands.ConnectionCommands.SendQuitAsync,

            // Key Commands
            Redis.Commands.KeyCommands.SendRandomKeyAsync,

            // Server Commands
            Redis.Commands.ServerCommands.SendBackgroundRewriteAOFAsync,
            Redis.Commands.ServerCommands.SendBackgroundSaveAsync,
            Redis.Commands.ServerCommands.SendDbSizeAsync,
            Redis.Commands.ServerCommands.SendFlushAllAsync,
            Redis.Commands.ServerCommands.SendFlushDbAsync,
            Redis.Commands.ServerCommands.SendInfoAsync,
            Redis.Commands.ServerCommands.SendLastSaveAsync,
            Redis.Commands.ServerCommands.SendMonitorAsync,
            Redis.Commands.ServerCommands.SendSaveAsync,
            Redis.Commands.ServerCommands.SendShutdownAsync,
            Redis.Commands.ServerCommands.SendSyncAsync,
            Redis.Commands.ServerCommands.SendTimeAsync,

            // Transaction Commands
            Redis.Commands.TransactionCommands.SendDiscardAsync,
            Redis.Commands.TransactionCommands.SendExecAsync,
            Redis.Commands.TransactionCommands.SendMultiAsync,
            Redis.Commands.TransactionCommands.SendUnWatchAsync
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
