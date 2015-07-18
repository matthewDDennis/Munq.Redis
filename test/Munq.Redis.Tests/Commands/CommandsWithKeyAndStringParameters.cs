using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Munq.Redis;
using Munq.Redis.Commands;
using Xunit;

namespace Munq.Redis.Tests.Commands
{
    public class CommandsWithKeyAndStringParameters
    {
        static readonly Encoding encoder = new UTF8Encoding();

        public enum Command
        {
            // Key Commands
            Rename,

            // Hash Commands
            HGet,

            // List Commands
            RPopLPush,

            // String Commands
            Append,
            GetSet,

        }

        bool[] StringRequired =
        {
            true,
            true,
            true,
            false,
            false
        };

        Func<RedisClient, string, string, Task>[] Methods = {
            // Key Commands
            (c, key, s) => c.SendRenameAsync(key, s),

            // Hash Commands
            (c, key, s) => c.SendHGetAsync(key, s),

            // List Commands
            (c, key, s) => c.SendRPopLPushAsync(key, s),

            // String Commands
            (c, key, s) => c.SendAppendAsync(key, s),
            (c, key, s) => c.SendGetSetAsync(key, s)
        };


        public static IEnumerable<object[]> Commands => ((int[])Enum.GetValues(typeof(Command)))
                                                         .Select(x => new object[] { (Command)x });

        [Theory, MemberData("Commands")]
        public async Task OkWithValidKeyAndString(Command command)
        {
            var stream = new MemoryStream();
            byte[] result;
            string cmdStr = command.ToString();

            string key = Guid.NewGuid().ToString();
            string aString = Guid.NewGuid().ToString();
            byte[] expected = encoder.GetBytes($"*3\r\n${cmdStr.Length}\r\n{cmdStr}\r\n${key.Length}\r\n{key}\r\n${aString.Length}\r\n{aString}\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Methods[(int)command](client, key, aString);

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Theory, MemberData("Commands")]
        public async Task WithValidKeyAndNullString(Command command)
        {
            var stream      = new MemoryStream();
            byte[] result;
            string cmdStr   = command.ToString();

            string key      = Guid.NewGuid().ToString();
            string aString  = null;
            byte[] expected = encoder.GetBytes($"*3\r\n${cmdStr.Length}\r\n{cmdStr}\r\n${key.Length}\r\n{key}\r\n$-1\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                if (StringRequired[(int)command])
                {
                    await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, key, aString));

                }
                else
                {
                    await Methods[(int)command](client, key, aString);

                    result = stream.ToArray();
                    Assert.Equal(expected, result);
                }
            }                
        }   

        [Theory, MemberData("Commands")]
        public async Task WithValidKeyAndEmptryString(Command command)
        {
            var stream      = new MemoryStream();
            byte[] result;
            string cmdStr   = command.ToString();

            string key      = Guid.NewGuid().ToString();
            string aString  = String.Empty;
            byte[] expected = encoder.GetBytes($"*3\r\n${cmdStr.Length}\r\n{cmdStr}\r\n${key.Length}\r\n{key}\r\n${aString.Length}\r\n{aString}\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                if (StringRequired[(int)command])
                {
                    await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, key, aString));

                }
                else
                {
                    await Methods[(int)command](client, key, aString);

                    result = stream.ToArray();
                    Assert.Equal(expected, result);
                }
            }
        }

        [Theory, MemberData("Commands")]
        public async Task ThrowsWithNullKey(Command command)
        {
            var stream = new MemoryStream();

            string key = null;
            string aString = Guid.NewGuid().ToString(); 

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                    await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, key, aString));
            }
        }

        [Theory, MemberData("Commands")]
        public async Task ThrowsWithEmptyKey(Command command)
        {
            var stream = new MemoryStream();

            string key = string.Empty;
            string aString = Guid.NewGuid().ToString();

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => Methods[(int)command](client, key, aString));
            }
        }
    }
}
