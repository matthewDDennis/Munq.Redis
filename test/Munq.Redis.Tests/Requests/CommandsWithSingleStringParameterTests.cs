using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Munq.Redis;
using Munq.Redis.Requests;
using Xunit.Extensions;

namespace Munq.Redis.Tests.Requests
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

        readonly Func<RedisClient, string, Task>[] Methods = {
            // Key Commands
            Redis.Requests.KeyCommands.SendDumpAsync,
            Redis.Requests.KeyCommands.SendExistsAsync,
            Redis.Requests.KeyCommands.SendKeysAsync,
            Redis.Requests.KeyCommands.SendPersistAsync,
            Redis.Requests.KeyCommands.SendPTTLAsync,
            Redis.Requests.KeyCommands.SendTTLAsync,
            Redis.Requests.KeyCommands.SendTypeAsync,

            // String Commands
            Redis.Requests.StringCommands.SendDecrAsync,
            Redis.Requests.StringCommands.SendGetAsync,
            Redis.Requests.StringCommands.SendIncrAsync,
            Redis.Requests.StringCommands.SendStrLenAsync,

            // List Commands
            Redis.Requests.ListCommands.SendLLenAsync,
            Redis.Requests.ListCommands.SendLPopAsync,
            Redis.Requests.ListCommands.SendRPopAsync,

            // Connection Commands
            Redis.Requests.ConnectionCommands.SendAuthAsync,
            Redis.Requests.ConnectionCommands.SendEchoAsync,

            // Hash Commands
            Redis.Requests.HashCommands.SendHGetAllAsync,
            Redis.Requests.HashCommands.SendHKeysAsync,
            Redis.Requests.HashCommands.SendHLenAsync,
            Redis.Requests.HashCommands.SendHValsAsync,

            // Set Commands
            Redis.Requests.SetCommands.SendSCardAsync,
            Redis.Requests.SetCommands.SendSMembersAsync,
            Redis.Requests.SetCommands.SendSPopAsync,
            Redis.Requests.SetCommands.SendSRandMemberAsync,

            // Sorted Set Commands
            Redis.Requests.SortedSetCommands.SendZCardAsync
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
