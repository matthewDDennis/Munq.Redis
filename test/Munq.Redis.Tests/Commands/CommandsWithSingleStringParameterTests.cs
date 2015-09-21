using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Munq.Redis;
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

        readonly Func<RedisClient, string, Task>[] Methods = {
            // Key Commands
            Keys.Commands.SendDumpAsync,
            Keys.Commands.SendExistsAsync,
            Keys.Commands.SendKeysAsync,
            Keys.Commands.SendPersistAsync,
            Keys.Commands.SendPTTLAsync,
            Keys.Commands.SendTTLAsync,
            Keys.Commands.SendTypeAsync,

            // String Commands
            Strings.Commands.SendDecrAsync,
            Strings.Commands.SendGetAsync,
            Strings.Commands.SendIncrAsync,
            Strings.Commands.SendStrLenAsync,

            // List Commands
            Lists.Commands.SendLLenAsync,
            Lists.Commands.SendLPopAsync,
            Lists.Commands.SendRPopAsync,

            // Connection Commands
            Redis.Connection.Commands.SendAuthAsync,
            Redis.Connection.Commands.SendEchoAsync,

            // Hash Commands
            Hashes.Commands.SendHGetAllAsync,
            Hashes.Commands.SendHKeysAsync,
            Hashes.Commands.SendHLenAsync,
            Hashes.Commands.SendHValsAsync,

            // Set Commands
            Sets.Commands.SendSCardAsync,
            Sets.Commands.SendSMembersAsync,
            Sets.Commands.SendSPopAsync,
            Sets.Commands.SendSRandMemberAsync,

            // Sorted Set Commands
            SortedSets.Commands.SendZCardAsync
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
