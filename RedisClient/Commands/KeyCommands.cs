using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class KeyCommands
    {
        public static Task SendDeleteAsync(this RedisClient client, params string[] keys)
        {
            return client.SendDeleteAsync((IEnumerable<string>)keys);
        }
        public static Task SendDeleteAsync(this RedisClient client, IEnumerable<string> keys)
        {
            return client.SendAsync("Del", (keys ?? new string[] { }));
        }
        public static Task SendDumpAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Dump", key);
        }
        public static Task SendEpireAtAsync(this RedisClient client,
                                                  string key, long unixTimeStamp)
        {
            return client.SendAsync("ExpireAt", key, unixTimeStamp);
        }
        public static Task SendExistsAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Exists", key);
        }
        public static Task SendExpireAsync(this RedisClient client,
                                                 string key, int seconds)
        {
            return client.SendAsync("Expire", key, seconds);
        }
        public static Task SendKeysAsync(this RedisClient client, string pattern)
        {
            return client.SendAsync("Keys", pattern);
        }
        public static Task SendMigrateAsync(this RedisClient client, string host,
                                                  int port, string key, int destination_db,
                                                  long timeoutMs)
        {
            return client.SendAsync("Migrate", host, port, key, destination_db, timeoutMs)
                        ;
        }
        public static Task SendMoveAsync(this RedisClient client, string key, int database)
        {
            return client.SendAsync("Move", key, database);
        }
        public static Task SendObjectEncodingAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Object", "Encoding", key);
        }
        public static Task SendObjectIdelTimeAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Object", "IdeTime", key);
        }
        public static Task SendObjectRefCountAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Object", "RefCount", key);
        }
        public static Task SendPersistAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Persist", key);
        }
        public static Task SendPExpireAsync(this RedisClient client,
                                                  string key, long milliseconds)
        {
            return client.SendAsync("PExpire", key, milliseconds);
        }
        public static Task SendPExpireAtAsync(this RedisClient client,
                                                    string key, long millisecondsTimestamp)
        {
            return client.SendAsync("PExpireAt", key, millisecondsTimestamp)
                        ;
        }
        public static Task SendPTTLAsync(this RedisClient client, string key)
        {
            return client.SendAsync("PTTL", key);
        }
        public static Task SendRandomKeyAsync(this RedisClient client)
        {
            return client.SendAsync("RandomKey");
        }
        public static Task SendRenameAsync(this RedisClient client,
                                                 string key, string newKey)
        {
            return client.SendAsync("Rename", key, newKey);
        }
        public static Task SendRenameXAsync(this RedisClient client,
                                                  string key, string newKey)
        {
            return client.SendAsync("RenameX", key, newKey);
        }
        public static Task SendRestoreAsync(this RedisClient client,
                                                  string key, long ttl, string serializeValue)
        {
            return client.SendAsync("Restore", key, ttl, serializeValue)
                        ;
        }
        public static Task SendScanAsync(this RedisClient client,
                                               long cursor, string pattern, long? count)
        {
            var parameters = new List<object>();

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

            return client.SendAsync("Scan", parameters);
        }
        public static Task SendTTLAsync(this RedisClient client, string key)
        {
            return client.SendAsync("TTL", key);
        }
        public static Task SendTypeAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Type", key);
        }
    }
}
