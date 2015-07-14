using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Munq.Redis;
using Munq.Redis.Commands;

namespace Munq.Redis.Tests.Commands
{
    public class KeyOnlyCommandsTests
    {
        static readonly Encoding encoder = new UTF8Encoding();

        Func<RedisClient, string, Task>[] KeyMethods = {
            // Key Commands
            Redis.Commands.KeyCommands.SendDumpAsync,
            Redis.Commands.KeyCommands.SendExistsAsync,
            Redis.Commands.KeyCommands.SendPersistAsync,
            Redis.Commands.KeyCommands.SendPTTLAsync,
            Redis.Commands.KeyCommands.SendTTLAsync,
            Redis.Commands.KeyCommands.SendTypeAsync,

            // String Commands
            Redis.Commands.StringCommands.SendDecrAsync,
            Redis.Commands.StringCommands.SendGetAsync,
            Redis.Commands.StringCommands.SendIncrAsync,
            Redis.Commands.StringCommands.SendStrLenAsync
        };

        public enum MethodName
        {
            // Key Commands
            Dump,
            Exists,
            Persist,
            PTTL,
            TTL,
            Type,

            // String Commands
            Decr,
            Get,
            Incr,
            StrLen
        }

        [Theory]
        [InlineData(MethodName.Dump)]
        [InlineData(MethodName.Exists)]
        [InlineData(MethodName.Persist)]
        [InlineData(MethodName.PTTL)]
        [InlineData(MethodName.TTL)]
        [InlineData(MethodName.Type)]
        [InlineData(MethodName.Decr)]
        [InlineData(MethodName.Get)]
        [InlineData(MethodName.Incr)]
        [InlineData(MethodName.StrLen)]
        public async Task OkWithAKey(MethodName methodName)
        {
            var stream = new MemoryStream();
            byte[] result;
            string command = methodName.ToString();

            byte[] expected = encoder.GetBytes($"*2\r\n${command.Length}\r\n{command}\r\n$4\r\nKey1\r\n");


            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await KeyMethods[(int)methodName](client, "Key1");

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(MethodName.Dump)]
        [InlineData(MethodName.Exists)]
        [InlineData(MethodName.Persist)]
        [InlineData(MethodName.PTTL)]
        [InlineData(MethodName.TTL)]
        [InlineData(MethodName.Type)]
        [InlineData(MethodName.Decr)]
        [InlineData(MethodName.Get)]
        [InlineData(MethodName.Incr)]
        [InlineData(MethodName.StrLen)]
        public async Task NoKeyThrows(MethodName methodName)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => KeyMethods[(int)methodName](client, null));
            }
        }

        [Theory]
        [InlineData(MethodName.Dump)]
        [InlineData(MethodName.Exists)]
        [InlineData(MethodName.Persist)]
        [InlineData(MethodName.PTTL)]
        [InlineData(MethodName.TTL)]
        [InlineData(MethodName.Type)]
        [InlineData(MethodName.Decr)]
        [InlineData(MethodName.Get)]
        [InlineData(MethodName.Incr)]
        [InlineData(MethodName.StrLen)]
        public async Task EmptyKeyThrows(MethodName methodName)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => KeyMethods[(int)methodName](client, string.Empty));
            }
        }
    }
}
