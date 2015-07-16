using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class SetCommands
    {
        public static Task SendSAddAsync(this RedisClient client,
                                                string key, params object[] values)
        {
            return client.SendSAddAsync(key, (IEnumerable<object>)values);
        }

        public static Task SendSAddAsync(this RedisClient client,
                                               string key, IEnumerable<object> values)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (values != null)
                parameters.AddRange(values);

            return client.SendAsync("SAdd", parameters);
        }

        public static Task SendSCardAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("SCard", key);
        }

        public static Task SendSDiffAsync(this RedisClient client, params string[] keys)
        {
            return client.SendSDiffAsync((IEnumerable<string>) keys);
        }

        public static Task SendSDiffAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("SDiff", keys);
        }

        public static Task SendSDiffStoreAsync(this RedisClient client,
                                                     string destKey, params string[] keys)
        {
            return client.SendSDiffStoreAsync(destKey, (IEnumerable<string>) keys);
        }

        public static Task SendSDiffStoreAsync(this RedisClient client,
                                                     string destKey, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            parameters.Add(destKey);
            if (keys != null)
                parameters.AddRange(keys);
            return client.SendAsync("SDiffStore", parameters);
        }

        public static Task SendSInterAsync(this RedisClient client, params string[] keys)
        {
            return client.SendSInterAsync((IEnumerable<string>) keys);
        }

        public static Task SendSInterAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("SInter", keys);
        }

        public static Task SendSInterStoreAsync(this RedisClient client,
                                                     string destKey, params string[] keys)
        {
            return client.SendSInterStoreAsync(destKey, (IEnumerable<string>) keys);
        }

        public static Task SendSInterStoreAsync(this RedisClient client,
                                                     string destKey, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            parameters.Add(destKey);
            if (keys != null)
                parameters.AddRange(keys);
            return client.SendAsync("SInterStore", parameters);
        }

        public static Task SendSIsMemberAsync(this RedisClient client, string key, object member)
        {
            return client.SendAsync("SIsMember", key, member);
        }

        public static Task SendSMembersAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("SMembers", key);
        }

        public static Task SendSMovesAsync(this RedisClient client,
                                                 string source, string destination, object member)
        {
            return client.SendAsync("SMove", source, destination, member);
        }

        public static Task SendSPopAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("SPop", key);
        }

        public static Task SendSRandMemberAsync(this RedisClient client, string key, long count)
        {
            return client.SendAsync("SRandMember", key, count);
        }

        public static Task SendSRandMemberAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("SRandMember", key);
        }

        public static Task SendSRemAsync(this RedisClient client,
                                               string key, params object[] members)
        {
            return client.SendSRemAsync(key, (IEnumerable<object>) members);
        }

        public static Task SendSRemAsync(this RedisClient client,
                                               string key, IEnumerable<object> members)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (members != null)
            {
                parameters.AddRange(members);
            }
                        return client.SendAsync("SRem", parameters);
        }

        public static Task SendSScanAsync(this RedisClient client,
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

            return client.SendAsync("SScan", parameters);
        }
        public static Task SendSUnionAsync(this RedisClient client, params string[] keys)
        {
            return client.SendSUnionAsync((IEnumerable<string>) keys);
        }

        public static Task SendSUnionAsync(this RedisClient client, IEnumerable<string> keys)
        {
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("SUnion", keys);
        }

        public static Task SendSUnionStoreAsync(this RedisClient client,
                                                     string destKey, params string[] keys)
        {
            return client.SendSUnionStoreAsync(destKey, (IEnumerable<string>) keys);
        }

        public static Task SendSUnionStoreAsync(this RedisClient client,
                                                     string destKey, IEnumerable<string> keys)
        {
            var parameters = new List<object>();
            parameters.Add(destKey);
            if (keys != null)
                parameters.AddRange(keys);
            return client.SendAsync("SUnionStore", parameters);
        }
    }
}
