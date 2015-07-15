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
            Redis.Commands.StringCommands.SendStrLenAsync,

            // List Commands
            Redis.Commands.ListCommands.SendLLenAsync,
            Redis.Commands.ListCommands.SendLPopAsync,
            Redis.Commands.ListCommands.SendRPopAsync,

            // Connection Commands
            Redis.Commands.ConnectionCommands.SendAuthAsync,
            Redis.Commands.ConnectionCommands.SendEchoAsync,

            // Hash Commands
            Redis.Commands.HashCommands.SendHGetAllAsync,
            Redis.Commands.HashCommands.SendHKeysAsync,
            Redis.Commands.HashCommands.SendHLenAsync,
            Redis.Commands.HashCommands.SendHValsAsync,

            // Set Commands
            Redis.Commands.SetCommands.SendSCardAsync,
            Redis.Commands.SetCommands.SendSMembersAsync,
            Redis.Commands.SetCommands.SendSPopAsync,
            Redis.Commands.SetCommands.SendSRandMemberAsync,


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
            StrLen,

            // List Commands
            LLen,
            LPop,
            RPop,

            // Connection Commands
            Auth,
            Echo,

            // Hash Commands
            HGetAll,
            HKeys,
            HLen,
            HVals,

            // Set Commands
            SCard,
            SMembers,
            SPop,
            SRandMember,

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
        [InlineData(MethodName.LLen)]
        [InlineData(MethodName.LPop)]
        [InlineData(MethodName.RPop)]
        [InlineData(MethodName.Auth)]
        [InlineData(MethodName.Echo)]
        [InlineData(MethodName.HGetAll)]
        [InlineData(MethodName.HKeys)]
        [InlineData(MethodName.HLen)]
        [InlineData(MethodName.HVals)]
        [InlineData(MethodName.SCard)]
        [InlineData(MethodName.SMembers)]
        [InlineData(MethodName.SPop)]
        [InlineData(MethodName.SRandMember)]
        public async Task OkWithAKey(MethodName methodName)
        {
            var stream = new MemoryStream();
            byte[] result;
            string command = methodName.ToString();

            string aString = Guid.NewGuid().ToString();
            byte[] expected = encoder.GetBytes($"*2\r\n${command.Length}\r\n{command}\r\n${aString.Length}\r\n{aString}\r\n");


            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await KeyMethods[(int)methodName](client, aString);

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
        [InlineData(MethodName.LLen)]
        [InlineData(MethodName.LPop)]
        [InlineData(MethodName.RPop)]
        [InlineData(MethodName.Auth)]
        [InlineData(MethodName.Echo)]
        [InlineData(MethodName.HGetAll)]
        [InlineData(MethodName.HKeys)]
        [InlineData(MethodName.HLen)]
        [InlineData(MethodName.HVals)]
        [InlineData(MethodName.SCard)]
        [InlineData(MethodName.SMembers)]
        [InlineData(MethodName.SPop)]
        [InlineData(MethodName.SRandMember)]
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
        [InlineData(MethodName.LLen)]
        [InlineData(MethodName.LPop)]
        [InlineData(MethodName.RPop)]
        [InlineData(MethodName.Auth)]
        [InlineData(MethodName.Echo)]
        [InlineData(MethodName.HGetAll)]
        [InlineData(MethodName.HKeys)]
        [InlineData(MethodName.HLen)]
        [InlineData(MethodName.HVals)]
        [InlineData(MethodName.SCard)]
        [InlineData(MethodName.SMembers)]
        [InlineData(MethodName.SPop)]
        [InlineData(MethodName.SRandMember)]
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
