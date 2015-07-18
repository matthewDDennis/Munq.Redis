using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class StringCommands
    {
        public static Task SendAppendAsync(this RedisClient client,
                                                 string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Append", key, value);
        }

        public static Task SendBitCountAsync(this RedisClient client,
                                                   string key, long? start, long? end)
        {
            if (end.HasValue && !start.HasValue)
            {
                start = 0;
            }
            var parameters = new List<object>();
            parameters.Add(key);
            if (start.HasValue)
            {
                parameters.Add(start.Value);
            }
            if (end.HasValue)
            {
                parameters.Add(end.Value);
            }
            return client.SendAsync("BitCount", parameters);
        }

        public static Task SendBitOpAsync(this RedisClient client,
                                                string operation, string destKey,
                                                params string[] keys)
        {
            return client.SendBitOpAsync(operation, destKey, (IEnumerable<string>) keys);
        }

        public static Task SendBitOpAsync(this RedisClient client,
                                                string operation, string destKey,
                                                IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            parameters.Add(operation);
            parameters.Add(destKey);
            parameters.Add(keys ?? new string[] { });

            return client.SendAsync("BitOp", parameters);
        }

        public static Task SendBitPosAsync(this RedisClient client,
                                                 string key, bool bitState, long? start, long? end)
        {
            if (end.HasValue && !start.HasValue)
            {
                start = 0;
            }
            var parameters = new List<object>();
            parameters.Add(key);
            parameters.Add(bitState);
            if (start.HasValue)
            {
                parameters.Add(start.Value);
            }
            if (end.HasValue)
            {
                parameters.Add(end.Value);
            }
            return client.SendAsync("BitPos", parameters);
        }

        public static Task SendDecrAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Decr", key);
        }

        public static Task SendDecrByAsync(this RedisClient client,
                                                 string key, long value)
        {
            return client.SendAsync("DecrBy", key, value);
        }

        public static Task SendGetAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Get", key);
        }

        public static Task SendGetBitAsync(this RedisClient client,
                                                 string key, long offset)
        {
            return client.SendAsync("GetBit", key, offset);
        }

        public static Task SendGetRangeAsync(this RedisClient client,
                                                   string key, long start, long end)
        {
            return client.SendAsync("GetRange", key, start, end);
        }

        public static Task SendGetSetAsync(this RedisClient client, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("GetSet", key, value);
        }

        public static Task SendIncrAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Incr", key);
        }

        public static Task SendIncrByAsync(this RedisClient client, string key,
                                                        long value)
        {
            return client.SendAsync("IncrBy", key, value);
        }

        public static Task SendIncrByFloatAsync(this RedisClient client,
                                                      string key, double value)
        {
            return client.SendAsync("IncrByFloat", key, value);
        }

        public static Task SendMGetAsync(this RedisClient client, params string[] keys)
        {
            return client.SendMGetAsync((IEnumerable<string>) keys);
        }

        public static Task SendMGetAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("MGet", keys);
        }

        public static Task SendMSetAsync(this RedisClient client,
                                           IEnumerable<KeyValuePair<string, object>> keyAndValues)
        {
            var parameters = new List<object>();
            if (keyAndValues != null)
            {
                foreach (var keyAndValue in keyAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }
            }
            return client.SendAsync("MSet", parameters);
        }

        public static Task SendMSetNXAsync(this RedisClient client,
                                            IEnumerable<KeyValuePair<string, object>> keyAndValues)
        {
            var parameters = new List<object>();
            if (keyAndValues != null)
            {
                foreach (var keyAndValue in keyAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }
            }
            return client.SendAsync("MSetNX", parameters);
        }

        public static Task SendPSetEXAsync(this RedisClient client,
                                                 string key, long msTimeout, object value)
        {
            return client.SendAsync("PSetEX", key, msTimeout, value);
        }

        public static Task SendSetAsync(this RedisClient client,
                                              string key, object value)
        {
            return SendSetAsync(client, key, value, null, null);
        }

        public static Task SendSetAsync(this RedisClient client, string key,
                                              object value,
                                              long? seconds, long? milliseconds,
                                              StringSetCommandOptions setOption = StringSetCommandOptions.Always)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            parameters.Add(value);

            if (seconds.HasValue)
            {
                parameters.Add("EX");
                parameters.Add(seconds.Value);
            }

            if (milliseconds.HasValue)
            {
                parameters.Add("PX");
                parameters.Add(milliseconds.Value);
            }

            if (setOption == StringSetCommandOptions.IfExists)
            {
                parameters.Add("XX");
            }
            else
            {
                if (setOption == StringSetCommandOptions.IfNotExists)
                {
                    parameters.Add("NX");
                }
            }

            return client.SendAsync("Set", parameters);
        }

        public static Task SendSetBitAsync(this RedisClient client,
                                                 string key, int offset, bool value)
        {
            return client.SendAsync("SetBit", key, offset, value);
        }

        public static Task SendSetEXAsync(this RedisClient client,
                                                string key, object value, long seconds)
        {
            return client.SendAsync("SetEX", key, seconds, value);
        }

        public static Task SendSetNXAsync(this RedisClient client,
                                                string key, object value)
        {
            return client.SendAsync("SetNX", key, value);
        }

        public static Task SendSetRangeAsync(this RedisClient client,
                                                   string key, long offset, object value)
        {
            return client.SendAsync("SetRange", key, offset, value);
        }
        public static Task SendStrLenAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("StrLen", key);
        }
    }
}
