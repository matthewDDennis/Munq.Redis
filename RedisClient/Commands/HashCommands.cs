using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class HashCommands
    {
        public static async Task SendHDelCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("HDel", key).ConfigureAwait(false);
        }

        public static async Task SendHDelCommandAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendCommandAsync("HDel", (keys ?? new string[]{})).ConfigureAwait(false);
        }

        public static async Task SendHExistsCommandAsync(this RedisClient client, string key, string field)
        {
            await client.SendCommandAsync("HExists", key, field).ConfigureAwait(false);
        }

        public static async Task SendHEGetCommandAsync(this RedisClient client, string key, string field)
        {
            await client.SendCommandAsync("HGet", key, field).ConfigureAwait(false);
        }

        public static async Task SendHEGetAllCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("HGetAll", key).ConfigureAwait(false);
        }

        public static async Task SendHIncrByCommandAsync(this RedisClient client, string key, string field, long increment)
        {
            await client.SendCommandAsync("HGetAll", key, field, increment).ConfigureAwait(false);
        }

        public static async Task SendHIncrByFloatCommandAsync(this RedisClient client, string key, string field, double increment)
        {
            await client.SendCommandAsync("HGetAll", key, field, increment).ConfigureAwait(false);
        }

        public static async Task SendHKeysCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("HKeys", key).ConfigureAwait(false);
        }

        public static async Task SendHLenCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("HLen", key).ConfigureAwait(false);
        }

        public static async Task SendHMGetCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("HMGet", key).ConfigureAwait(false);
        }

        public static async Task SendHMGetCommandAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendCommandAsync("HMGet", (keys ?? new string[] { })).ConfigureAwait(false);
        }

        public async static Task SendHMSetCommandAsync(this RedisClient client, string key,
                                           IEnumerable<KeyValuePair<string, object>> fieldAndValues)
        {
            List<object> parameters = new List<object>();
            parameters.Add(key);
            if (fieldAndValues != null)
                foreach (var keyAndValue in fieldAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }

            await client.SendCommandAsync("HMSet", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendHSetCommandAsync(this RedisClient client, string key, string field, object value)
        {
            await client.SendCommandAsync("HSet", key, field, value).ConfigureAwait(false);
        }

        public async static Task SendHSetNXCommandAsync(this RedisClient client, string key, string field, object value)
        {
            await client.SendCommandAsync("HSetNX", key, field, value).ConfigureAwait(false);
        }

        public static async Task SendHValsCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("HVals", key).ConfigureAwait(false);
        }

        public async static Task SendHScanCommandAsync(this RedisClient client, string key, long cursor,
                                                      string pattern, long? count)
        {
            List<object> parameters = new List<object>();

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

            await client.SendCommandAsync("Scan", parameters).ConfigureAwait(false);
        }
    }
}
