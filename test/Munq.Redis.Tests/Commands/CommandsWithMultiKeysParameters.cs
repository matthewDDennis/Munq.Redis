using System;
using System.IO;
using Xunit;

using Munq.Redis.Commands;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Tests.Commands.KeyCommands
{
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
            Redis.Commands.StringCommands.SendMGetAsync
        };

        [Theory]
        [InlineData(Command.Del)]
        [InlineData(Command.HDel)]
        [InlineData(Command.HMGet)]
        [InlineData(Command.SDiff)]
        [InlineData(Command.SInter)]
        [InlineData(Command.SUnion)]
        [InlineData(Command.MGet)]
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

        [Theory]
        [InlineData(Command.Del)]
        [InlineData(Command.HDel)]
        [InlineData(Command.HMGet)]
        [InlineData(Command.SDiff)]
        [InlineData(Command.SInter)]
        [InlineData(Command.SUnion)]
        [InlineData(Command.MGet)]
        public async Task DelManyKeys(Command command)
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


        [Theory]
        [InlineData(Command.Del)]
        [InlineData(Command.HDel)]
        [InlineData(Command.HMGet)]
        [InlineData(Command.SDiff)]
        [InlineData(Command.SInter)]
        [InlineData(Command.SUnion)]
        [InlineData(Command.MGet)]
        public async Task DelNullKeysThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, null));
            }
        }

        [Theory]
        [InlineData(Command.Del)]
        [InlineData(Command.HDel)]
        [InlineData(Command.HMGet)]
        [InlineData(Command.SDiff)]
        [InlineData(Command.SInter)]
        [InlineData(Command.SUnion)]
        [InlineData(Command.MGet)]
        public async Task DelEmptyArrayOfKeysThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, new string[] { }));
            }
        }

        [Theory]
        [InlineData(Command.Del)]
        [InlineData(Command.HDel)]
        [InlineData(Command.HMGet)]
        [InlineData(Command.SDiff)]
        [InlineData(Command.SInter)]
        [InlineData(Command.SUnion)]
        [InlineData(Command.MGet)]
        public async Task DelEmptyKeyThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, new string[] { string.Empty }));
            }
        }
    }
}
