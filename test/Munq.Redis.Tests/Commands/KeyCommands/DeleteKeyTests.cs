using System;
using System.IO;
using Xunit;

using Munq.Redis.Commands;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Tests.Commands.KeyCommands
{
    public class DeleteKeyTests
    {
        static readonly Encoding encoder = new UTF8Encoding();
        [Fact]
        public async Task DelOneKey()
        {
            var stream = new MemoryStream();
            byte[] result;
            var expected = encoder.GetBytes("*2\r\n$3\r\nDel\r\n$4\r\nKey1\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await client.SendDeleteAsync("Key1");

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task DelTwoKeys()
        {
            var stream = new MemoryStream();
            byte[] result;
            var expected = encoder.GetBytes("*3\r\n$3\r\nDel\r\n$4\r\nKey1\r\n$4\r\nKey2\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await client.SendDeleteAsync("Key1", "Key2");
                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task DelArrayOfKeys()
        {
            var stream = new MemoryStream();
            byte[] result;
            var expected = encoder.GetBytes("*4\r\n$3\r\nDel\r\n$4\r\nKey1\r\n$4\r\nKey2\r\n$4\r\nKey3\r\n");

            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await client.SendDeleteAsync(new string[] { "Key1", "Key2", "Key3" });
                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task DelNoKeysThrows()
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync());
            }
        }

        [Fact]
        public async Task DelEmptyArrayOfKeysThrows()
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync(new string[] { }));
            }
        }

        [Fact]
        public async Task DelEmptyKeyThrows()
        {
            var stream = new MemoryStream();
            using (var client = new RedisClient(new RedisStreamConnection(stream)))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync(string.Empty));
            }
        }
    }
}
