using System;
using System.IO;
using Xunit;

using Munq.Redis.Commands;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis.Tests.Commands.KeyCommands
{
    /// <summary>
    /// Tests the commands with the format COMMAND Key [Key2 [Key3 ...]] are
    /// serialized correctly and handle error as expected.
    /// </summary>
    public class CommandsWithMultiKeysParameters
    {
        static readonly Encoding encoder = new UTF8Encoding();

        public enum Command
        {
            // Key Commands
            Del,

            // Hash Commands
            HDel,
            HMGet,

            // Set Commands
            SDiff,
            SInter,
            SUnion,

            // String Commands
            MGet,

            // Transaction Commands
            Watch
        }

        Func<RedisClient, string[], Task>[] Methods = {
            // Key Commands
            Redis.Commands.KeyCommands.SendDeleteAsync,

            // Hash Commands
            Redis.Commands.HashCommands.SendHDelAsync,
            Redis.Commands.HashCommands.SendHMGetAsync,

            // Set Commands
            Redis.Commands.SetCommands.SendSDiffAsync,
            Redis.Commands.SetCommands.SendSInterAsync,
            Redis.Commands.SetCommands.SendSUnionAsync,

            // String Commands
            Redis.Commands.StringCommands.SendMGetAsync,

            // Transaction Commands
            Redis.Commands.TransactionCommands.SendWatchKeysAsync
        };

        public static IEnumerable<object[]> Commands => ((int[])Enum.GetValues(typeof(Command)))
                                                         .Select(x => new object[] { (Command)x });

        [Theory, MemberData("Commands")]
        public async Task OneKey(Command command)
        {
            var stream = new MemoryStream();
            byte[] result;
            string cmdStr = command.ToString();

            string aString = Guid.NewGuid().ToString();
            byte[] expected = encoder.GetBytes($"*2\r\n${cmdStr.Length}\r\n{cmdStr}\r\n${aString.Length}\r\n{aString}\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Methods[(int)command](client, new string[] {aString});

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Theory, MemberData("Commands")]
        public async Task ManyKeys(Command command)
        {
            var stream = new MemoryStream();
            byte[] result;
            string cmdStr = command.ToString();

            string aString1 = Guid.NewGuid().ToString();
            string aString2 = Guid.NewGuid().ToString();
            string aString3 = Guid.NewGuid().ToString();
            byte[] expected = encoder.GetBytes(
                $"*4\r\n${cmdStr.Length}\r\n{cmdStr}\r\n${aString1.Length}\r\n{aString1}\r\n${aString2.Length}\r\n{aString2}\r\n${aString3.Length}\r\n{aString3}\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Methods[(int)command](client, new string[] { aString1, aString2, aString3 });

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }


        [Theory, MemberData("Commands")]
        public async Task NullKeysThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, null));
            }
        }

        [Theory, MemberData("Commands")]
        public async Task EmptyArrayOfKeysThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, new string[] { }));
            }
        }

        [Theory, MemberData("Commands")]
        public async Task EmptyKeyThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, new string[] { string.Empty }));
            }
        }
    }
}
