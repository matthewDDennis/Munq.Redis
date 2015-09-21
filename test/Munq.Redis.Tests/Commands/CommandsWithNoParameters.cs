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
            Redis.Connection.Commands.SendPingAsync,
            Redis.Connection.Commands.SendQuitAsync,

            // Key Commands
            Redis.Keys.Commands.SendRandomKeyAsync,

            // Server Commands
            Server.Commands.SendBackgroundRewriteAOFAsync,
            Server.Commands.SendBackgroundSaveAsync,
            Server.Commands.SendDbSizeAsync,
            Server.Commands.SendFlushAllAsync,
            Server.Commands.SendFlushDbAsync,
            Server.Commands.SendInfoAsync,
            Server.Commands.SendLastSaveAsync,
            Server.Commands.SendMonitorAsync,
            Server.Commands.SendSaveAsync,
            Server.Commands.SendShutdownAsync,
            Server.Commands.SendSyncAsync,
            Server.Commands.SendTimeAsync,

            // Transaction Commands
            Transactions.Commands.SendDiscardAsync,
            Transactions.Commands.SendExecAsync,
            Transactions.Commands.SendMultiAsync,
            Transactions.Commands.SendUnWatchAsync
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
