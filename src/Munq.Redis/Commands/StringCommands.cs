using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class StringCommands
    {
        public async static Task SendAppendAsync(this RedisClient client,
                                                 string key, string value)
        {
            await client.SendAsync("Append", key, value).ConfigureAwait(false);
        }

        public async static Task SendBitCountAsync(this RedisClient client,
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
            await client.SendAsync("BitCount", parameters).ConfigureAwait(false);
        }
        public async static Task SendBitOpAsync(this RedisClient client,
                                                string operation, string destKey,
        params string[] keys)
        {
            await client.SendBitOpAsync(operation, destKey, (IEnumerable<string>) keys);
        }
        public async static Task SendBitOpAsync(this RedisClient client,
                                                string operation, string destKey,
                                                IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            parameters.Add(operation);
            parameters.Add(destKey);
            parameters.Add(keys ?? new string[] { });

            await client.SendAsync("BitOp", parameters).ConfigureAwait(false);
        }
        public async static Task SendBitPosAsync(this RedisClient client,
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
            await client.SendAsync("BitPos", parameters).ConfigureAwait(false);
        }
        public async static Task SendDecrAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Decr", key).ConfigureAwait(false);
        }
        public async static Task SendDecrByAsync(this RedisClient client,
                                                 string key, long value)
        {
            await client.SendAsync("DecrBy", key, value).ConfigureAwait(false);
        }
        public async static Task SendGetAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Get", key).ConfigureAwait(false);
        }
        public async static Task SendGetBitAsync(this RedisClient client,
                                                 string key, long offset)
        {
            await client.SendAsync("GetBit", key, offset).ConfigureAwait(false);
        }
        public async static Task SendGetRangeAsync(this RedisClient client,
                                                   string key, long start, long end)
        {
            await client.SendAsync("GetRange", key, start, end).ConfigureAwait(false);
        }
        public async static Task SendGetSetAsync(this RedisClient client, string key, string value)
        {
            await client.SendAsync("GetSet", key, value).ConfigureAwait(false);
        }
        public async static Task SendIncrAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Incr", key).ConfigureAwait(false);
        }
        public async static Task SendIncrByAsync(this RedisClient client, string key,
                                                        long value)
        {
            await client.SendAsync("IncrBy", key, value).ConfigureAwait(false);
        }
        public async static Task SendIncrByFloatAsync(this RedisClient client,
                                                      string key, double value)
        {
            await client.SendAsync("IncrByFloat", key, value).ConfigureAwait(false);
        }
        public async static Task SendMGetAsync(this RedisClient client, params string[] keys)
        {
            await client.SendMGetAsync((IEnumerable<string>) keys).ConfigureAwait(false);
        }
        public async static Task SendMGetAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("MGet", (keys ?? new string[] { })).ConfigureAwait(false);
        }
        public async static Task SendMSetAsync(this RedisClient client,
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
            await client.SendAsync("MSet", parameters).ConfigureAwait(false);
        }
        public async static Task SendMSetNXAsync(this RedisClient client,
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
            await client.SendAsync("MSetNX", parameters).ConfigureAwait(false);
        }
        public async static Task SendPSetEXAsync(this RedisClient client,
                                                 string key, long msTimeout, object value)
        {
            await client.SendAsync("PSetEX", key, msTimeout, value).ConfigureAwait(false);
        }
        public async static Task SendSetAsync(this RedisClient client,
                                              string key, object value)
        {
            await SendSetAsync(client, key, value, null, null).ConfigureAwait(false);
        }
        public async static Task SendSetAsync(this RedisClient client, string key,
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
            await client.SendAsync("Set", parameters).ConfigureAwait(false);
        }
        public async static Task SendSetBitAsync(this RedisClient client,
                                                 string key, int offset, bool value)
        {
            await client.SendAsync("SetBit", key, offset, value).ConfigureAwait(false);
        }
        public async static Task SendSetEXAsync(this RedisClient client,
                                                string key, object value, long seconds)
        {
            await client.SendAsync("SetEX", key, seconds, value).ConfigureAwait(false);
        }
        public async static Task SendSetNXAsync(this RedisClient client,
                                                string key, object value)
        {
            await client.SendAsync("SetNX", key, value).ConfigureAwait(false);
        }
        public async static Task SendSetRangeAsync(this RedisClient client,
                                                   string key, long offset, object value)
        {
            await client.SendAsync("SetRange", key, offset, value).ConfigureAwait(false);
        }
        public async static Task SendStrLenAsync(this RedisClient client, string key)
        {
            await client.SendAsync("StrLen", key).ConfigureAwait(false);
        }
    }
}
