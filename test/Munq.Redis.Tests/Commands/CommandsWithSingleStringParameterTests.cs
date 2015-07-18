using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Munq.Redis;
using Munq.Redis.Commands;
using Xunit.Extensions;

namespace Munq.Redis.Tests.Commands
{
    public class CommandsWithSingleStringParameterTests
    {
        static readonly Encoding encoder = new UTF8Encoding();

        public enum Command
        {
            // Key Commands
            Dump,
            Exists,
            Keys,
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

            // Sorted Set Commands
            ZCard,
        }

        Func<RedisClient, string, Task>[] Methods = {
            // Key Commands
            Redis.Commands.KeyCommands.SendDumpAsync,
            Redis.Commands.KeyCommands.SendExistsAsync,
            Redis.Commands.KeyCommands.SendKeysAsync,
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

            // Sorted Set Commands
            Redis.Commands.SortedSetCommands.SendZCardAsync
        };

        public static IEnumerable<object[]> Commands =>  ((int[])Enum.GetValues(typeof(Command)))
                                                         .Select(x => new object[] { (Command)x }); 

        [Theory, MemberData("Commands")]
        public async Task OkWithAKey(Command command)
        {
            var stream = new MemoryStream();
            byte[] result;
            string cmdStr = command.ToString();

            string aString = Guid.NewGuid().ToString();
            byte[] expected = encoder.GetBytes($"*2\r\n${cmdStr.Length}\r\n{cmdStr}\r\n${aString.Length}\r\n{aString}\r\n");


            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Methods[(int)command](client, aString);

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Theory, MemberData("Commands")]
        public async Task NoKeyThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, null));
            }
        }

        [Theory, MemberData("Commands")]
        public async Task EmptyKeyThrows(Command command)
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, string.Empty));
            }
        }
    }
}
