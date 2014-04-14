using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public enum SetOptions
    {
        Always,
        IfExists,
        IfNotExists
    }

    public static class StringCommands
    {
        public async static Task SendAppend(this RedisClient client, string key, string value)
        {
            await client.SendCommand("Append", key, value).ConfigureAwait(false);
        }

        public async static Task SendBitCount(this RedisClient client, string key, long? start, long? end)
        {
            if (end.HasValue && !start.HasValue)
                start = 0;

            List<object> parameters = new List<object>();
            parameters.Add(key);
            if (start.HasValue)
                parameters.Add(start.Value);
            if (end.HasValue)
                parameters.Add(end.Value);

            await client.SendCommand("BitCount", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendBitOp(this RedisClient client, string operation, string destKey, IEnumerable<string> keys)
        {
            List<object> parameters = new List<object>();
            parameters.Add(operation);
            parameters.Add(destKey);
            parameters.Add(keys);

            await client.SendCommand("BitOp", parameters.ToArray());
        }

        public async static Task SendBitPos(this RedisClient client, string key, bool bitState, long? start, long? end)
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

            await client.SendCommand("BitPos", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendDecr(this RedisClient client, string key)
        {
            await client.SendCommand("Decr", key).ConfigureAwait(false);
        }

        public async static Task SendDecrBy(this RedisClient client, string key, long value)
        {
            await client.SendCommand("DecrBy", key, value).ConfigureAwait(false);
        }

        public async static Task SendGet(this RedisClient client, string key)
        {
            await client.SendCommand("Get", key).ConfigureAwait(false);
        }

        public async static Task SendGetBit(this RedisClient client, string key, long offset)
        {
            await client.SendCommand("GetBit", key, offset).ConfigureAwait(false);
        }

        public async static Task SendGetRange(this RedisClient client, string key, long start, long end)
        {
            await client.SendCommand("GetRange", key, start, end).ConfigureAwait(false);
        }

        public async static Task SendGetSet(this RedisClient client, string key, string value)
        {
            await client.SendCommand("GetSet", key, value).ConfigureAwait(false);
        }

        public async static Task SendIncr(this RedisClient client, string key)
        {
            await client.SendCommand("Incr", key).ConfigureAwait(false);
        }

        public async static Task SendIncrBy(this RedisClient client, string key, long value)
        {
            await client.SendCommand("IncrBy", key, value).ConfigureAwait(false);
        }

        public async static Task SendIncrByFloat(this RedisClient client, string key, double value)
        {
            await client.SendCommand("IncrByFloat", key, value).ConfigureAwait(false);
        }

        public async static Task SendMGet(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null)
                keys = new string[] { };

            await client.SendCommand("MGet", keys.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendMSet(this RedisClient client, IEnumerable<KeyValuePair<string, object>> keyAndValues)
        {
            List<object> parameters = new List<object>();
            if (keyAndValues!= null)
                foreach (var keyAndValue in keyAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }

            await client.SendCommand("MSet", parameters.ToArray()).ConfigureAwait(false);
        }


        public async static Task SendMSetNX(this RedisClient client, IEnumerable<KeyValuePair<string, object>> keyAndValues)
        {
            List<object> parameters = new List<object>();
            if (keyAndValues != null)
                foreach (var keyAndValue in keyAndValues)
                {
                    parameters.Add(keyAndValue.Key);
                    parameters.Add(keyAndValue.Value);
                }

            await client.SendCommand("MSetNX", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendPSetEX(this RedisClient client, string key, long millisecondsTimeout, object value)
        {
            await client.SendCommand("PSetEX", key, millisecondsTimeout, value).ConfigureAwait(false);   
        }

        public async static Task SendSet(this RedisClient client, string key, object value, long? seconds, 
                                         long? milliseconds, SetOptions setOption = SetOptions.Always)
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

            if (setOption == SetOptions.IfExists)
                parameters.Add("XX");
            else if (setOption == SetOptions.IfNotExists)
                parameters.Add("NX");

            await client.SendCommand("Set", parameters.ToArray()).ConfigureAwait(false);
        }

        public async static Task SendSet(this RedisClient client, string key, object value)
        {
            await SendSet(client, key, value, null, null).ConfigureAwait(false);
        }

        public async static Task SendSetBit(this RedisClient client, string key, int offset, bool value)
        {
            await client.SendCommand("SetBit", key, offset, value).ConfigureAwait(false);
        }

        public async static Task SendSetEX(this RedisClient client, string key, object value, long seconds)
        {
            await client.SendCommand("SetEX", key, seconds, value).ConfigureAwait(false);
        }

        public async static Task SendSetNX(this RedisClient client, string key, object value)
        {
            await client.SendCommand("SetNX", key, value).ConfigureAwait(false);
        }

        // TODO: value may have to be string
        public async static Task SendSetRange(this RedisClient client, string key, long offset, object value)
        {
            await client.SendCommand("SetRange", key, offset, value).ConfigureAwait(false); ;
        }

        public async static Task SendStrLen(this RedisClient client, string key)
        {
            await client.SendCommand("StrLen", key).ConfigureAwait(false);
        }
    }
}
