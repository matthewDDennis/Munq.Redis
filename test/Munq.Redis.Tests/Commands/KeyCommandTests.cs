using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

using Munq.Redis.Commands;
using System.Text;

namespace Munq.Redis.Tests.Commands
{
    public class KeyCommandTests
    {
        static readonly Encoding encoder = new UTF8Encoding();
        [Fact]
        public void DelOneKey()
        {
            var stream = new MemoryStream();
            var expected = encoder.GetBytes("*2\r\n$3\r\nDel\r\n$4\r\nKey1\r\n");

            var client = RedisClientFactory.Create(stream);
            client.SendDeleteAsync("Key1");

            var result = stream.ToArray();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelTwoKeys()
        {
            var stream = new MemoryStream();
            var client = RedisClientFactory.Create(stream);
            var expected = encoder.GetBytes("*3\r\n$3\r\nDel\r\n$4\r\nKey1\r\n$4\r\nKey2\r\n");

            client.SendDeleteAsync("Key1", "Key2");

            var result = stream.ToArray();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelArrayOfKeys()
        {
            var stream = new MemoryStream();
            var client = RedisClientFactory.Create(stream);
            var expected = encoder.GetBytes("*4\r\n$3\r\nDel\r\n$4\r\nKey1\r\n$4\r\nKey2\r\n$4\r\nKey3\r\n");

            client.SendDeleteAsync(new string[] { "Key1", "Key2", "Key3" });

            var result = stream.ToArray();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void DelNoKeysThrows()
        {
            var stream = new MemoryStream();
            var client = RedisClientFactory.Create(stream);
            Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync());
        }

        [Fact]
        public void DelEmptyArrayOfKeysThrows()
        {
            var stream = new MemoryStream();
            var client = RedisClientFactory.Create(stream);
            Assert.ThrowsAsync<ArgumentNullException>(() => client.SendDeleteAsync(new string[] { }));
        }

    }
}
