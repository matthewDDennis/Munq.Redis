using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class StringCommands
    {
        public async static Task SendAppendCommandAsync(this RedisClient client, string key, 
                                                        string value)
        {
            await client.SendCommandAsync("Append", key, value).ConfigureAwait(false);
        }

        public async static Task SendBitCountCommandAsync(this RedisClient client, string key, 
                                                          long? start, long? end)
        {
            if (end.HasValue && !start.HasValue)
                start = 0;

            List<object> parameters = new List<object>();
            parameters.Add(key);
            if (start.HasValue)
                parameters.Add(start.Value);
            if (end.HasValue)
                parameters.Add(end.Value);

            await client.SendCommandAsync("BitCount", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendBitOpCommandAsync(this RedisClient client, string operation, 
                                                       string destKey, IEnumerable<string> keys)
        {
            List<object> parameters = new List<object>();
            parameters.Add(operation);
            parameters.Add(destKey);
            parameters.Add(keys ?? new string[] {});

            await client.SendCommandAsync("BitOp", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendBitPosCommandAsync(this RedisClient client, string key, 
                                                        bool bitState, long? start, long? end)
        {
            if (end.HasValue && !start.HasValue)
                start = 0;

            List<object> parameters = new List<object>();
            parameters.Add(key);
            parameters.Add(bitState);
            if (start.HasValue)
                parameters.Add(start.Value);
            if (end.HasValue)
                parameters.Add(end.Value);

            await client.SendCommandAsync("BitPos", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendDecrCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Decr", key).ConfigureAwait(false);
        }

        public async static Task SendDecrByCommandAsync(this RedisClient client, string key, 
                                                        long value)
        {
            await client.SendCommandAsync("DecrBy", key, value).ConfigureAwait(false);
        }

        public async static Task SendGetCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Get", key).ConfigureAwait(false);
        }

        public async static Task SendGetBitCommandAsync(this RedisClient client, string key, 
                                                        long offset)
        {
            await client.SendCommandAsync("GetBit", key, offset).ConfigureAwait(false);
        }

        public async static Task SendGetRangeCommandAsync(this RedisClient client, string key, 
                                                          long start, long end)
        {
            await client.SendCommandAsync("GetRange", key, start, end).ConfigureAwait(false);
        }

        public async static Task SendGetSetCommandAsync(this RedisClient client, string key, 
                                                        string value)
        {
            await client.SendCommandAsync("GetSet", key, value).ConfigureAwait(false);
        }

        public async static Task SendIncrCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Incr", key).ConfigureAwait(false);
        }

        public async static Task SendIncrByCommandAsync(this RedisClient client, string key, 
                                                        long value)
        {
            await client.SendCommandAsync("IncrBy", key, value).ConfigureAwait(false);
        }

        public async static Task SendIncrByFloatCommandAsync(this RedisClient client, string key, 
                                                             double value)
        {
            await client.SendCommandAsync("IncrByFloat", key, value).ConfigureAwait(false);
        }

        public async static Task SendMGetCommandAsync(this RedisClient client, 
                                                      IEnumerable<string> keys)
        {
            await client.SendCommandAsync("MGet", (keys ?? new string[] {}).ToArray())
                        .ConfigureAwait(false);
        }

        public async static Task SendMSetCommandAsync(this RedisClient client, 
                                           IEnumerable<KeyValuePair<string, object>> keyAndValues)
        {
            List<object> parameters = new List<object>();
            if (keyAndValues!= null)
                foreach (var keyAndValue in keyAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }

            await client.SendCommandAsync("MSet", parameters.ToArray()).ConfigureAwait(false);
        }


        public async static Task SendMSetNXCommandAsync(this RedisClient client, 
                                            IEnumerable<KeyValuePair<string, object>> keyAndValues)
        {
            List<object> parameters = new List<object>();
            if (keyAndValues != null)
                foreach (var keyAndValue in keyAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }

            await client.SendCommandAsync("MSetNX", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendPSetEXCommandAsync(this RedisClient client, string key, 
                                                        long millisecondsTimeout, object value)
        {
            await client.SendCommandAsync("PSetEX", key, millisecondsTimeout, value)
                        .ConfigureAwait(false);   
        }

        public async static Task SendSetCommandAsync(this RedisClient client, string key, 
                                                     object value, 
                                                     long? seconds, long? milliseconds, 
                                                     SetCommandOptions setOption = SetCommandOptions.Always)
        {
            List<object> parameters = new List<object>();
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

            if (setOption == SetCommandOptions.IfExists)
                parameters.Add("XX");
            else if (setOption == SetCommandOptions.IfNotExists)
                parameters.Add("NX");

            await client.SendCommandAsync("Set", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendSetCommandAsync(this RedisClient client, string key, 
                                                     object value)
        {
            await SendSetCommandAsync(client, key, value, null, null).ConfigureAwait(false);
        }

        public async static Task SendSetBitCommandAsync(this RedisClient client, string key, 
                                                        int offset, bool value)
        {
            await client.SendCommandAsync("SetBit", key, offset, value).ConfigureAwait(false);
        }

        public async static Task SendSetEXCommandAsync(this RedisClient client, string key, 
                                                       object value, long seconds)
        {
            await client.SendCommandAsync("SetEX", key, seconds, value).ConfigureAwait(false);
        }

        public async static Task SendSetNXCommandAsync(this RedisClient client, string key, 
                                                       object value)
        {
            await client.SendCommandAsync("SetNX", key, value).ConfigureAwait(false);
        }

        // TODO: value may have to be string
        public async static Task SendSetRangeCommandAsync(this RedisClient client, string key, 
                                                          long offset, object value)
        {
            await client.SendCommandAsync("SetRange", key, offset, value).ConfigureAwait(false);
        }

        public async static Task SendStrLenCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("StrLen", key).ConfigureAwait(false);
        }
    }
}
