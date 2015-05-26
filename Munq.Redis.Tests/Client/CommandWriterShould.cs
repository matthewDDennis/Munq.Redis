using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;
using Munq.Redis;
using System.Threading.Tasks;

namespace Munq.Redis.Tests.Client
{
    /// <summary>
    /// All Redis Commands are sent as an array of Redis Bulk Strings.
    /// </summary>
    [TestClass]
    public class CommandWriterShould
    {
        static readonly Encoding encoder = new UTF8Encoding();

        [TestMethod]
        public async Task SendAnArrayWithOneElementForACommandWithoutParameters()
        {
            var stream        = new MemoryStream();
            var commandWriter = new CommandWriter(stream);

            await commandWriter.WriteRedisCommandAsync("DoIt");

            var expectedCommandString = "*1\r\n$4\r\nDoIt\r\n";
            var expectedBytes = encoder.GetBytes(expectedCommandString);

            var bytes = stream.ToArray();
            Assert.IsNotNull(bytes);
            CollectionAssert.AreEqual(expectedBytes, bytes);
        }
    }
}
