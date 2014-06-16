using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class SortedSetCommands
    {
        public static async Task SendZAddSync(this RedisClient client, string key,
                                              long score, object value)
        {
            await client.SendAsync("ZAdd", key, score, value).ConfigureAwait(false);
        }

        public static async Task SendZAddSync(this RedisClient client, string key,
                                              IEnumerable<KeyValuePair<long, object>> scoreAndValues)
        {
            var parameters = new List<object>();
            parameters.Add(key);
            if (scoreAndValues != null)
            {
                foreach (var scoreAndValue in scoreAndValues)
                {
                    parameters.Add(scoreAndValue.Key);
                    parameters.Add(scoreAndValue.Value);
                }
            }

            await client.SendAsync("ZAdd", parameters).ConfigureAwait(false);
        }
        public static async Task SendZCardAsync(this RedisClient client, string key)
        {
            await client.SendAsync("ZCard", key).ConfigureAwait(false);
        }

        public static async Task SendZCountAsync(this RedisClient client, string key, string min, string max)
        {
            await client.SendAsync("ZAdd", key, min, max).ConfigureAwait(false);
        }

        public static async Task SendZIncrByAsync(this RedisClient client, string key, long increment, object member)
        {
            await client.SendAsync("ZAdd", key, increment, member).ConfigureAwait(false);
        }

        public static async Task SendZInterStoreAsync(this RedisClient client, string destination, 
                                                     long numKeys, IEnumerable<string> keys,
                                                     IEnumerable<long> weights, 
                                                     SetAggregate aggregate = SetAggregate.Default)
        {
            List<object> parameters = new List<object>();
            parameters.Add(destination);
            parameters.Add(numKeys);
            parameters.AddRange(keys);
            if (weights != null)
            {
                parameters.Add("Weights");
                parameters.AddRange(weights.Select(w => (object)w));
            }
            if (aggregate != SetAggregate.Default)
            {
                parameters.Add("Aggregate");
                parameters.Add(aggregate.ToString());
            }
            await client.SendAsync("ZInterStore", parameters).ConfigureAwait(false);
        }


        public static async Task SendZUnionStoreAsync(this RedisClient client, string destination, 
                                                     long numKeys, IEnumerable<string> keys,
                                                     IEnumerable<long> weights, 
                                                     SetAggregate aggregate = SetAggregate.Default)
        {
            List<object> parameters = new List<object>();
            parameters.Add(destination);
            parameters.Add(numKeys);
            parameters.AddRange(keys);
            if (weights != null)
            {
                parameters.Add("Weights");
                parameters.AddRange(weights.Select(w => (object)w));
            }
            if (aggregate != SetAggregate.Default)
            {
                parameters.Add("Aggregate");
                parameters.Add(aggregate.ToString());
            }
            await client.SendAsync("ZUnionStore", parameters).ConfigureAwait(false);
        }

   }
}
