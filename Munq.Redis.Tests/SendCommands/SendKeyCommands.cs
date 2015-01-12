using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Munq.Redis.Commands;
using Munq.Redis.Responses;
using System.Threading.Tasks;

namespace Munq.Redis.Tests.SendCommands
{
    [TestClass]
    public class SendKeyCommands
    {
        const string key = "AKey";
        const string keyData = "some date";

        static RedisClient _client;

        [ClassInitialize]
        public static void TestClassInit(TestContext context)
        {
            _client = new RedisClient();
        }

        [TestInitialize]
        public void TestSetup()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [ClassCleanup]
        public static void TestClassCleanup()
        {
            _client.Dispose();
        }

        [TestMethod]
        public async Task ExistsCommandOnNonExistingKeyReturns0()
        {
            const string key = "AKey";
            await EmptyDatabase();
            await _client.SendExistsAsync(key);
            long response = await GetIntegerResponse();
            Assert.AreEqual(0L, response);
        }

        [TestMethod]
        public async Task ExistsCommandOnExistingKeyReturns1()
        {
            await EmptyDatabase();
            await SetKey(key, keyData);
            await _client.SendExistsAsync(key);
            long response = await GetIntegerResponse();
            Assert.AreEqual(1L, response);
        }

        [TestMethod]
        public async Task CanDumpAndRestoreKey()
        {
            await EmptyDatabase();
            await SetKey(key, keyData);
            await _client.SendDumpAsync(key);
            var dumpData = await _client.ReadResponseAsync();
            Assert.IsNotNull(dumpData);
            Assert.IsInstanceOfType(dumpData, typeof(RedisBulkString));
            var bulkString = (RedisBulkString)dumpData;
            Assert.IsTrue(bulkString.Value.Length > 0);
            await DelKey(key);
            Assert.IsFalse(await KeyExists(key));
            await _client.SendRestoreAsync(key, 0, bulkString.Value);
            await GetOkResponse();
            string restoredData = await GetKey(key);
            Assert.AreEqual(keyData, restoredData);

        }

        async Task EmptyDatabase()
        {
            await _client.SendFlushDbAsync(); // clean out the database
            await GetOkResponse();
        }

        async Task SetKey(string key, string data)
        {
            await _client.SendSetAsync(key, data);
            await GetOkResponse();
        }

        async Task DelKey(string key)
        {
            await _client.SendDeleteAsync(key);
            var response = await GetIntegerResponse();
        }

        async Task<long> GetIntegerResponse()
        {
            var response = await _client.ReadResponseAsync();
            Assert.IsInstanceOfType(response, typeof(long));
            return (long)response;
        }

        async Task<string> GetStringResponse()
        {
            var response = await _client.ReadResponseAsync();
            Assert.IsInstanceOfType(response, typeof(string));
            return (string)response;
        }

        async Task<bool> GetOkResponse()
        {
            var response = await GetStringResponse();
            Assert.AreEqual("OK", response);
            return true;
        }

        async Task<bool> KeyExists(string key)
        {
            await _client.SendExistsAsync(key);
            long response = await GetIntegerResponse();
            return 1L == response;
        }

        async Task<string> GetKey(string key)
        {
            await _client.SendGetAsync(key);
            return await _client.ExpectBulkStringAsync();
        }
    }
}
