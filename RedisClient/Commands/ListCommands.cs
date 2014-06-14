using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Munq.Redis.Commands
{
    /// <summary>
    /// This class is responsible for sending the Redis commands that are related to 
    /// the Redis List objects.  The commands are implemented as extension method to
    /// the RedisClient class.  Methods are in the form of SendAAAAAAsync.
    /// </summary>
    public static class ListCommands
    {
        public static async Task SendBLPopAsync(this RedisClient client,
                                                long timeout, params string[] keys)
        {
            await client.SendBLPopAsync(timeout, (IEnumerable<string>)keys).ConfigureAwait(false);
        }
        public static async Task SendBLPopAsync(this RedisClient client,
                                                long timeout, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            if (keys != null)
            {
                parameters.AddRange(keys);
            }
            parameters.Add(timeout);
            await client.SendAsync("BLPop", parameters).ConfigureAwait(false);
        }
        public static async Task SendBRPopAsync(this RedisClient client,
                                                long timeout, params string[] keys)
        {
            await client.SendBRPopAsync(timeout, (IEnumerable<string>)keys);
        }
        public static async Task SendBRPopAsync(this RedisClient client,
                                                long timeout, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            if (keys != null)
            {
                parameters.AddRange(keys);
            }
            parameters.Add(timeout);
            await client.SendAsync("BRPop", parameters).ConfigureAwait(false);
        }
        public static async Task SendBRPopLPushAsync(this RedisClient client, long timeout,
                                                     string source, string destination)
        {
            await client.SendAsync("BRPopLPush", source, destination, timeout).ConfigureAwait(false);
        }
        public static async Task SendLIndexAsync(this RedisClient client,
                                                 string key, long index)
        {
            await client.SendAsync("LIndex", key, index).ConfigureAwait(false);
        }
        public static async Task SendLInsertAsync(this RedisClient client, string key,
                                                  InsertOptions option, object pivot,
                                                  object value)
        {
            await client.SendAsync("LInsert", key, option, pivot, value).ConfigureAwait(false);
        }
        public static async Task SendLLenAsync(this RedisClient client, string key)
        {
            await client.SendAsync("LLen", key).ConfigureAwait(false);
        }
        public static async Task SendLPopAsync(this RedisClient client, string key)
        {
            await client.SendAsync("LPop", key).ConfigureAwait(false);
        }
        public static async Task SendLPushAsync(this RedisClient client,
                                                string key, params object[] values)
        {
            await client.SendLPushAsync(key, (IEnumerable<string>)values).ConfigureAwait(false);
        }
        public static async Task SendLPushAsync(this RedisClient client,
                                                string key, IEnumerable<object> values)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
            {
                parameters.AddRange(values);
            }
                        await client.SendAsync("LPush", parameters).ConfigureAwait(false);
        }
        public static async Task SendLPushXAsync(this RedisClient client,
                                                 string key, object value)
        {
            await client.SendAsync("LPushX", key, value).ConfigureAwait(false);
        }
        public static async Task SendLRangeAsync(this RedisClient client,
                                                 string key, long start, long stop)
        {
            await client.SendAsync("LRange", key, start, stop).ConfigureAwait(false);
        }
        public static async Task SendLRemAsync(this RedisClient client,
                                               string key, long count, object value)
        {
            await client.SendAsync("LRange", key, count, value).ConfigureAwait(false);
        }
        public static async Task SendLSetAsync(this RedisClient client,
                                               string key, long index, object value)
        {
            await client.SendAsync("LSet", key, index, value).ConfigureAwait(false);
        }
        public static async Task SendLTrimAsync(this RedisClient client,
                                                string key, long start, long stop)
        {
            await client.SendAsync("LTrim", key, start, stop).ConfigureAwait(false);
        }
        public static async Task SendRPopAsync(this RedisClient client, string key)
        {
            await client.SendAsync("RPop", key).ConfigureAwait(false);
        }
        public static async Task SendRPopLPushAsync(this RedisClient client,
                                                    string source, string destination)
        {
            await client.SendAsync("RPopLPush", source, destination).ConfigureAwait(false);
        }
        public static async Task SendRPushAsync(this RedisClient client,
                                                string key, params object[] values)
        {
            await client.SendRPushAsync( key, (IEnumerable<string>)values).ConfigureAwait(false);
        }
        public static async Task SendRPushAsync(this RedisClient client,
                                                string key, IEnumerable<object> values)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
            {
                parameters.AddRange(values);
            }
                        await client.SendAsync("RPush", parameters).ConfigureAwait(false);
        }
        public static async Task SendRPushXAsync(this RedisClient client,
                                                 string key, object value)
        {
            await client.SendAsync("RPushX", key, value).ConfigureAwait(false);
        }
    }
}
