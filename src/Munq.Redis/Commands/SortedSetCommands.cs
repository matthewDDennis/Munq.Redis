using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.SortedSets
{
    public static class Commands
    {
        public static Task SendZAddAsync(this RedisClient client, string key,
                                              long score, object value)
        {
            return client.SendAsync("ZAdd", key, score, value);
        }

        public static Task SendZAddAsync(this RedisClient client, string key,
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

            return client.SendAsync("ZAdd", parameters);
        }
        public static Task SendZCardAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("ZCard", key);
        }

        public static Task SendZCountAsync(this RedisClient client, string key, string min, string max)
        {
            return client.SendAsync("ZAdd", key, min, max);
        }

        public static Task SendZIncrByAsync(this RedisClient client, string key, long increment, object member)
        {
            return client.SendAsync("ZAdd", key, increment, member);
        }

        public static Task SendZInterStoreAsync(this RedisClient client, string destination,
                                                     long numKeys, IEnumerable<string> keys,
                                                     IEnumerable<long> weights,
                                                     SetAggregate aggregate = SetAggregate.Default)
        {
            string cmdStr = "ZInterStore";
            return SendSetOpAsync(client, destination, numKeys, keys, weights, aggregate, cmdStr);
        }

        public static Task SendZUnionStoreAsync(this RedisClient client, string destination,
                                                     long numKeys, IEnumerable<string> keys,
                                                     IEnumerable<long> weights,
                                                     SetAggregate aggregate = SetAggregate.Default)
        {
            string cmdStr = "ZUnionStore";
            return SendSetOpAsync(client, destination, numKeys, keys, weights, aggregate, cmdStr);
        }
        static Task SendSetOpAsync(RedisClient client, string destination, 
                                                 long numKeys, IEnumerable<string> keys, 
                                                 IEnumerable<long> weights, SetAggregate aggregate, 
                                                 string cmdStr)
        {
            var parameters = new List<object>();
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
            return client.SendAsync(cmdStr, parameters);
        }
   }
}
