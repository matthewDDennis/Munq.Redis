using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class SetCommands
    {
         public async static Task SendSAddAsync(this RedisClient client, 
                                                string key, params object[] values)
        {
            await client.SendSAddAsync(key, (IEnumerable<string>)values).ConfigureAwait(false);
        }

        public async static Task SendSAddAsync(this RedisClient client, 
                                               string key, IEnumerable<object> values)
        {
            List<object> parameters = new List<object>();
            parameters.Add(key);
            parameters.AddRange(values);

            await client.SendAsync("SAdd", parameters).ConfigureAwait(false);
        }

        public static async Task SendSCardAsync(this RedisClient client, string key)
        {
            await client.SendAsync("SCard", key).ConfigureAwait(false);
        }

        public static async Task SendSDiffAsync(this RedisClient client, params string[] keys)
        {
            await client.SendSDiffAsync((IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendSDiffAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("SDiff", keys).ConfigureAwait(false);
        }

        public static async Task SendSDiffStoreAsync(this RedisClient client, 
                                                     string destKey, params string[] keys)
        {
            await client.SendSDiffStoreAsync(destKey, (IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendSDiffStoreAsync(this RedisClient client, 
                                                     string destKey, IEnumerable<string> keys)
        {
            List<object> parameters = new List<object>();
            parameters.Add(destKey);
            if (keys != null)
                parameters.AddRange(keys);

            await client.SendAsync("SDiffStore", parameters).ConfigureAwait(false);
        }

         public static async Task SendSInterAsync(this RedisClient client, params string[] keys)
        {
            await client.SendSInterAsync((IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendSInterAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("SInter", keys).ConfigureAwait(false);
        }

        public static async Task SendSInterStoreAsync(this RedisClient client, 
                                                     string destKey, params string[] keys)
        {
            await client.SendSInterStoreAsync(destKey, (IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendSInterStoreAsync(this RedisClient client, 
                                                     string destKey, IEnumerable<string> keys)
        {
            List<object> parameters = new List<object>();
            parameters.Add(destKey);
            if (keys != null)
                parameters.AddRange(keys);

            await client.SendAsync("SInterStore", parameters).ConfigureAwait(false);
        }

        public static async Task SendSIsMemberAsync(this RedisClient client, string key, object member)
        {
            await client.SendAsync("SIsMember", key, member).ConfigureAwait(false);
        }

        public static async Task SendSMembersAsync(this RedisClient client, string key)
        {
            await client.SendAsync("SMembers", key).ConfigureAwait(false);
        }

        public static async Task SendSMovesAsync(this RedisClient client, 
                                                 string source, string destination, object member)
        {
            await client.SendAsync("SMove", source, destination, member).ConfigureAwait(false);
        }

        public static async Task SendSPopAsync(this RedisClient client, string key)
        {
            await client.SendAsync("SPop", key).ConfigureAwait(false);
        }

         public static async Task SendSRandMemberAsync(this RedisClient client, 
                                                       string key, long? count = null)
        {
            if (count.HasValue)
                await client.SendAsync("SRandMember", key, count).ConfigureAwait(false);
            else
                await client.SendAsync("SRandMember", key).ConfigureAwait(false);

        }

         public static async Task SendSRemAsync(this RedisClient client, 
                                                string key, params object[] members)
        {
            await client.SendSRemAsync(key, (IEnumerable<object>) members).ConfigureAwait(false);
        }

        public static async Task SendSRemAsync(this RedisClient client, 
                                               string key, IEnumerable<object> members)
        {
            List<object> parameters = new List<object>();
            parameters.Add(key);
            if (members != null)
                parameters.AddRange(members);

            await client.SendAsync("SRem", parameters).ConfigureAwait(false);
        }

        public static async Task SendSUnionAsync(this RedisClient client, params string[] keys)
        {
            await client.SendSUnionAsync((IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendSUnionAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("SUnion", keys).ConfigureAwait(false);
        }

        public static async Task SendSUnionStoreAsync(this RedisClient client, 
                                                     string destKey, params string[] keys)
        {
            await client.SendSUnionStoreAsync(destKey, (IEnumerable<string>) keys).ConfigureAwait(false);
        }

        public static async Task SendSUnionStoreAsync(this RedisClient client, 
                                                     string destKey, IEnumerable<string> keys)
        {
            List<object> parameters = new List<object>();
            parameters.Add(destKey);
            if (keys != null)
                parameters.AddRange(keys);

            await client.SendAsync("SUnionStore", parameters).ConfigureAwait(false);
        }

        public async static Task SendSScanAsync(this RedisClient client, 
                                                string key, long cursor, string pattern, long? count)
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

            await client.SendAsync("SScan", parameters).ConfigureAwait(false);
        }
    }
}
