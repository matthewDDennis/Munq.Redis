using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class ListCommands
    {
        public static async Task SendBLPopCommandAsync(this RedisClient client, string key, 
                                                       long timeout)
        {
            await client.SendCommandAsync("BLPop", key, timeout).ConfigureAwait(false);
        }

        public static async Task SendBLPopCommandAsync(this RedisClient client, 
                                                       IEnumerable<string> keys, long timeout)
        {
            List<object> parameters = new List<object>();
            if (keys != null)
                parameters.AddRange(keys);
            parameters.Add(timeout);
            await client.SendCommandAsync("BLPop", parameters).ConfigureAwait(false);
        }

        public static async Task SendBRPopCommandAsync(this RedisClient client, string key, 
                                                       long timeout)
        {
            await client.SendCommandAsync("BRPop", key, timeout).ConfigureAwait(false);
        }

        public static async Task SendBRPopCommandAsync(this RedisClient client, 
                                                       IEnumerable<string> keys, long timeout)
        {
            List<object> parameters = new List<object>();
            if (keys != null)
                parameters.AddRange(keys);
            parameters.Add(timeout);
            await client.SendCommandAsync("BRPop", parameters).ConfigureAwait(false);
        }

        public static async Task SendBRPopLPushCommandAsync(this RedisClient client, string source, 
                                                            string destination, long timeout)
        {
            await client.SendCommandAsync("BRPopLPush", source, destination, timeout)
                        .ConfigureAwait(false);
        }

        public static async Task SendLIndexCommandAsync(this RedisClient client, string key, 
                                                        long index)
        {
            await client.SendCommandAsync("LIndex", key, index).ConfigureAwait(false);
        }

        public static async Task SendLInsertCommandAsync(this RedisClient client, string key, 
                                                         InsertOptions option, object pivot, 
                                                         object value)
        {
            await client.SendCommandAsync("LInsert", key, option, pivot, value)
                        .ConfigureAwait(false);
        }

        public static async Task SendLLenCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("LLen", key).ConfigureAwait(false);
        }

        public static async Task SendLPopCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("LPop", key).ConfigureAwait(false);
        }

        public static async Task SendLPushCommandAsync(this RedisClient client, string key, 
                                                       object value)
        {
            await client.SendCommandAsync("LPush", key, value).ConfigureAwait(false);
        }

        public static async Task SendLPushCommandAsync(this RedisClient client, string key,
                                                       IEnumerable<object> values)
        {
            List<object> parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
                parameters.AddRange(values);
            await client.SendCommandAsync("LPush", values.ToArray()).ConfigureAwait(false);
        }

        public static async Task SendLPushXCommandAsync(this RedisClient client, string key,
                                                       object value)
        {
            await client.SendCommandAsync("LPushX", key, value).ConfigureAwait(false);
        }

        public static async Task SendLRangeCommandAsync(this RedisClient client, string key,
                                                        long start, long stop)
        {
            await client.SendCommandAsync("LRange", key, start, stop).ConfigureAwait(false);
        }

        public static async Task SendLRemCommandAsync(this RedisClient client, string key,
                                                        long count, object value)
        {
            await client.SendCommandAsync("LRange", key, count, value).ConfigureAwait(false);
        }

        public static async Task SendLSetCommandAsync(this RedisClient client, string key,
                                                        long index, object value)
        {
            await client.SendCommandAsync("LSet", key, index, value).ConfigureAwait(false);
        }

        public static async Task SendLTrimCommandAsync(this RedisClient client, string key,
                                                        long start, long stop)
        {
            await client.SendCommandAsync("LTrim", key, start, stop).ConfigureAwait(false);
        }

        public static async Task SendRPopCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("RPop", key).ConfigureAwait(false);
        }

        public static async Task SendRPopLPushCommandAsync(this RedisClient client, string source,
                                                            string destination)
        {
            await client.SendCommandAsync("RPopLPush", source, destination)
                        .ConfigureAwait(false);
        }

        public static async Task SendRPushCommandAsync(this RedisClient client, string key,
                                                   object value)
        {
            await client.SendCommandAsync("RPush", key, value).ConfigureAwait(false);
        }

        public static async Task SendRPushCommandAsync(this RedisClient client, string key,
                                                       IEnumerable<object> values)
        {
            List<object> parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
                parameters.AddRange(values);
            await client.SendCommandAsync("RPush", values.ToArray()).ConfigureAwait(false);
        }

        public static async Task SendRPushXCommandAsync(this RedisClient client, string key,
                                                       object value)
        {
            await client.SendCommandAsync("RPushX", key, value).ConfigureAwait(false);
        }
    }
}
