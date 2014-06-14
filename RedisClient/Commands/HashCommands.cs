using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Munq.Redis.Commands
{
    public static class HashCommands
    {
        public static async Task SendHDelAsync(this RedisClient client, params string[] keys)
        {
            await client.SendHDelAsync((IEnumerable<string>)keys).ConfigureAwait(false);
        }
        public static async Task SendHDelAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("HDel", (keys ?? new string[] { })).ConfigureAwait(false);
        }
        public static async Task SendHExistsAsync(this RedisClient client, string key, string field)
        {
            await client.SendAsync("HExists", key, field).ConfigureAwait(false);
        }
        public static async Task SendHGetAllAsync(this RedisClient client, string key)
        {
            await client.SendAsync("HGetAll", key).ConfigureAwait(false);
        }
        public static async Task SendHGetAsync(this RedisClient client, string key, string field)
        {
            await client.SendAsync("HGet", key, field).ConfigureAwait(false);
        }
        public static async Task SendHIncrByAsync(this RedisClient client,
                                                  string key, string field, long increment)
        {
            await client.SendAsync("HIncrBy", key, field, increment).ConfigureAwait(false);
        }
        public static async Task SendHIncrByFloatAsync(this RedisClient client,
                                                       string key, string field, double increment)
        {
            await client.SendAsync("HIncrByFloat", key, field, increment).ConfigureAwait(false);
        }
        public static async Task SendHKeysAsync(this RedisClient client, string key)
        {
            await client.SendAsync("HKeys", key).ConfigureAwait(false);
        }
        public static async Task SendHLenAsync(this RedisClient client, string key)
        {
            await client.SendAsync("HLen", key).ConfigureAwait(false);
        }
        public static async Task SendHMGetAsync(this RedisClient client, params string[] keys)
        {
            await client.SendHMGetAsync((IEnumerable<string>)keys).ConfigureAwait(false);
        }
        public static async Task SendHMGetAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("HMGet", (keys ?? new string[] { })).ConfigureAwait(false);
        }
        public async static Task SendHMSetAsync(this RedisClient client, string key,
                                           IEnumerable<KeyValuePair<string, object>> fieldAndValues)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (fieldAndValues != null)
            {
                foreach (var keyAndValue in fieldAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }
            }
                        await client.SendAsync("HMSet", parameters).ConfigureAwait(false);
        }
        public async static Task SendHScanAsync(this RedisClient client,
                                                string key, long cursor, string pattern, long? count)
        {
            var parameters = new List<object>();

            parameters.Add(key);
            parameters.Add(cursor);

            if (!string.IsNullOrWhiteSpace(pattern))
            {
                parameters.Add("MATCH");
                parameters.Add(pattern);
            }

            if (count.HasValue)
            {
                parameters.Add("COUNT");
                parameters.Add(count.Value);
            }

            await client.SendAsync("Scan", parameters).ConfigureAwait(false);
        }
        public async static Task SendHSetAsync(this RedisClient client,
                                               string key, string field, object value)
        {
            await client.SendAsync("HSet", key, field, value).ConfigureAwait(false);
        }
        public async static Task SendHSetNXAsync(this RedisClient client,
                                                 string key, string field, object value)
        {
            await client.SendAsync("HSetNX", key, field, value).ConfigureAwait(false);
        }
        public static async Task SendHValsAsync(this RedisClient client, string key)
        {
            await client.SendAsync("HVals", key).ConfigureAwait(false);
        }
    }
}
