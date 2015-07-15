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
        public static Task SendBLPopAsync(this RedisClient client,
                                                long timeout, params string[] keys)
        {
            return client.SendBLPopAsync(timeout, (IEnumerable<string>)keys);
        }

        public static Task SendBLPopAsync(this RedisClient client,
                                                long timeout, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            if (keys != null)
            {
                parameters.AddRange(keys);
            }
            parameters.Add(timeout);
            return client.SendAsync("BLPop", parameters);
        }

        public static Task SendBRPopAsync(this RedisClient client,
                                                long timeout, params string[] keys)
        {
            return client.SendBRPopAsync(timeout, (IEnumerable<string>)keys);
        }

        public static Task SendBRPopAsync(this RedisClient client,
                                                long timeout, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            if (keys != null)
                parameters.AddRange(keys);
            parameters.Add(timeout);
            return client.SendAsync("BRPop", parameters);
        }

        public static Task SendBRPopLPushAsync(this RedisClient client, long timeout,
                                                     string source, string destination)
        {
            return client.SendAsync("BRPopLPush", source, destination, timeout);
        }

        public static Task SendLIndexAsync(this RedisClient client,
                                                 string key, long index)
        {
            return client.SendAsync("LIndex", key, index);
        }

        public static Task SendLInsertAsync(this RedisClient client, string key,
                                                  InsertOptions option, object pivot,
                                                  object value)
        {
            return client.SendAsync("LInsert", key, option, pivot, value);
        }

        public static Task SendLLenAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("LLen", key);
        }

        public static Task SendLPopAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("LPop", key);
        }

        public static Task SendLPushAsync(this RedisClient client,
                                                string key, params object[] values)
        {
            return client.SendLPushAsync(key, (IEnumerable<string>)values);
        }

        public static Task SendLPushAsync(this RedisClient client,
                                                string key, IEnumerable<object> values)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
                parameters.AddRange(values);
            return client.SendAsync("LPush", parameters);
        }

        public static Task SendLPushXAsync(this RedisClient client,
                                                 string key, object value)
        {
            return client.SendAsync("LPushX", key, value);
        }

        public static Task SendLRangeAsync(this RedisClient client,
                                                 string key, long start, long stop)
        {
            return client.SendAsync("LRange", key, start, stop);
        }

        public static Task SendLRemAsync(this RedisClient client,
                                               string key, long count, object value)
        {
            return client.SendAsync("LRange", key, count, value);
        }

        public static Task SendLSetAsync(this RedisClient client,
                                               string key, long index, object value)
        {
            return client.SendAsync("LSet", key, index, value);
        }

        public static Task SendLTrimAsync(this RedisClient client,
                                                string key, long start, long stop)
        {
            return client.SendAsync("LTrim", key, start, stop);
        }

        public static Task SendRPopAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("RPop", key);
        }

        public static Task SendRPopLPushAsync(this RedisClient client,
                                                    string source, string destination)
        {
            return client.SendAsync("RPopLPush", source, destination);
        }

        public static Task SendRPushAsync(this RedisClient client,
                                                string key, params object[] values)
        {
            return client.SendRPushAsync( key, (IEnumerable<string>)values);
        }

        public static Task SendRPushAsync(this RedisClient client,
                                                string key, IEnumerable<object> values)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
                parameters.AddRange(values);
            return client.SendAsync("RPush", parameters);
        }

        public static Task SendRPushXAsync(this RedisClient client,
                                                 string key, object value)
        {
            return client.SendAsync("RPushX", key, value);
        }
    }
}
