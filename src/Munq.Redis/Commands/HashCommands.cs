using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Hashes
{
    public static class Commands
    {
        public static Task SendHDelAsync(this RedisClient client, params string[] keys)
        {
            return client.SendHDelAsync((IEnumerable<string>)keys);
        }

        public static Task SendHDelAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("HDel", keys);
        }

        public static Task SendHExistsAsync(this RedisClient client, string key, string field)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentNullException(nameof(field));

            return client.SendAsync("HExists", key, field);
        }

        public static Task SendHGetAllAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("HGetAll", key);
        }

        public static Task SendHGetAsync(this RedisClient client, string key, string field)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentNullException(nameof(field));

            return client.SendAsync("HGet", key, field);
        }

        public static Task SendHIncrByAsync(this RedisClient client,
                                                  string key, string field, long increment)
        {
            return client.SendAsync("HIncrBy", key, field, increment);
        }

        public static Task SendHIncrByFloatAsync(this RedisClient client,
                                                       string key, string field, double increment)
        {
            return client.SendAsync("HIncrByFloat", key, field, increment);
        }

        public static Task SendHKeysAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("HKeys", key);
        }

        public static Task SendHLenAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("HLen", key);
        }

        public static Task SendHMGetAsync(this RedisClient client, params string[] keys)
        {
            return client.SendHMGetAsync((IEnumerable<string>)keys);
        }

        public static Task SendHMGetAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("HMGet", keys);
        }

        public static Task SendHMSetAsync(this RedisClient client, string key,
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

            return client.SendAsync("HMSet", parameters);
        }

        public static Task SendHScanAsync(this RedisClient client,
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

            return client.SendAsync("Scan", parameters);
        }

        public static Task SendHSetAsync(this RedisClient client,
                                               string key, string field, object value)
        {
            return client.SendAsync("HSet", key, field, value);
        }

        public static Task SendHSetNXAsync(this RedisClient client,
                                                 string key, string field, object value)
        {
            return client.SendAsync("HSetNX", key, field, value);
        }

        public static Task SendHStrLenAsync(this RedisClient client, string key, string field)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentNullException(nameof(field));

            return client.SendAsync("HStrLen", key, field);
        }

        public static Task SendHValsAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("HVals", key);
        }
    }
}
