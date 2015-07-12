using System;
using System.IO;
using Xunit;

using Munq.Redis.Commands;
using System.Text;

namespace Munq.Redis.Tests.Commands.KeyCommands
{
    public class DeleteKeyTests
    {
        static readonly Encoding encoder = new UTF8Encoding();
        [Fact]
        public void DelOneKey()
        {
            var stream = new MemoryStream();
            byte[] result;
            var expected = encoder.GetBytes("*2\r\n$3\r\nDel\r\n$4\r\nKey1\r\n");

            using (var client = RedisClientFactory.Create(stream))
            {
                client.SendDeleteAsync("Key1");

                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelTwoKeys()
        {
            var stream = new MemoryStream();
            byte[] result;
            var expected = encoder.GetBytes("*3\r\n$3\r\nDel\r\n$4\r\nKey1\r\n$4\r\nKey2\r\n");

            using (var client = RedisClientFactory.Create(stream))
            {
                client.SendDeleteAsync("Key1", "Key2");
                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelArrayOfKeys()
        {
            var stream = new MemoryStream();
            byte[] result;
            var expected = encoder.GetBytes("*4\r\n$3\r\nDel\r\n$4\r\nKey1\r\n$4\r\nKey2\r\n$4\r\nKey3\r\n");

            using (var client = RedisClientFactory.Create(stream))
            {
                client.SendDeleteAsync(new string[] { "Key1", "Key2", "Key3" });
                result = stream.ToArray();
            }
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelNoKeysThrows()
        {
            var stream = new MemoryStream();
            using (var client = RedisClientFactory.Create(stream))
            {
                Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync());
            }
        }

        [Fact]
        public void DelEmptyArrayOfKeysThrows()
        {
            var stream = new MemoryStream();
            using (var client = RedisClientFactory.Create(stream))
            {
                Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync(new string[] { }));
            }
        }

        [Fact]
        public void DelEmptyKeyThrows()
        {
            var stream = new MemoryStream();
            using (var client = RedisClientFactory.Create(stream))
            {
                Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync(string.Empty));
            }
        }
    }
}
