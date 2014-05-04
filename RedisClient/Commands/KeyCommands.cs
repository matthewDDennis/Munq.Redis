using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    public static class KeyCommands
    {
        public async static Task SendDeleteAsync(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendAsync("Del", (keys ?? new string[]{}).ToArray())
                        .ConfigureAwait(false);
        }

        public async static Task SendDeleteAsync(this RedisClient client, params string[] keys)
        {
            await client.SendDeleteAsync((IEnumerable<string>)keys).ConfigureAwait(false);
        }

        public async static Task SendDumpAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Dump", key).ConfigureAwait(false);
        }

        public async static Task SendExistsAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Exists", key).ConfigureAwait(false);
        }

        public async static Task SendExpireAsync(this RedisClient client, 
                                                 string key, int seconds)
        {
            await client.SendAsync("Expire", key, seconds).ConfigureAwait(false);
        }

        public async static Task SendEpireAtAsync(this RedisClient client, 
                                                  string key, long unixTimeStamp)
        {
            await client.SendAsync("ExpireAt", key, unixTimeStamp).ConfigureAwait(false);
        }

        public async static Task SendKeysAsync(this RedisClient client, string pattern)
        {
            await client.SendAsync("Keys", pattern).ConfigureAwait(false);
        }

        public async static Task SendMigrateAsync(this RedisClient client, string host, 
                                                  int port, string key, int destination_db, 
                                                  long timeoutMs)
        {
            await client.SendAsync("Migrate", host, port, key, destination_db, timeoutMs)
                        .ConfigureAwait(false);
        }

        public async static Task SendMoveAsync(this RedisClient client, string key, int database)
        {
            await client.SendAsync("Move", key, database).ConfigureAwait(false);
        }

        public async static Task SendObjectRefCountAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Object", "RefCount", key).ConfigureAwait(false);
        }

        public async static Task SendObjectEncodingAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Object", "Encoding", key).ConfigureAwait(false);
        }

        public async static Task SendObjectIdelTimeAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Object", "IdeTime", key).ConfigureAwait(false);
        }

        public async static Task SendPersistAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Persist", key).ConfigureAwait(false);
        }

        public async static Task SendPExpireAsync(this RedisClient client, 
                                                  string key, long milliseconds)
        {
            await client.SendAsync("PExpire", key, milliseconds).ConfigureAwait(false);
        }

        public async static Task SendPExpireAtAsync(this RedisClient client, 
                                                    string key, long millisecondsTimestamp)
        {
            await client.SendAsync("PExpireAt", key, millisecondsTimestamp)
                        .ConfigureAwait(false);
        }

        public async static Task SendPTTLAsync(this RedisClient client, string key)
        {
            await client.SendAsync("PTTL", key).ConfigureAwait(false);
        }

        public async static Task SendRandomKeyAsync(this RedisClient client)
        {
            await client.SendAsync("RandomKey").ConfigureAwait(false);
        }

        public async static Task SendRenameAsync(this RedisClient client, 
                                                 string key, string newKey)
        {
            await client.SendAsync("Rename", key, newKey).ConfigureAwait(false);
        }

        public async static Task SendRenameXAsync(this RedisClient client, 
                                                  string key, string newKey)
        {
            await client.SendAsync("RenameX", key, newKey).ConfigureAwait(false);
        }

        public async static Task SendRestoreAsync(this RedisClient client, 
                                                  string key, long ttl, string serializeValue)
        {
            await client.SendAsync("Restore", key, ttl, serializeValue)
                        .ConfigureAwait(false);
        }

        // TODO: finish this one.  Its a little more complicated than most.
        //public async static Task SenSort(this RedisClient client, string key, ...)
        //{
        //    await client.SendCommand("Sort", key, ...).ConfigureAwait(false);
        //}

        public async static Task SendScanAsync(this RedisClient client, 
                                               long cursor, string pattern, long? count)
        {
            List<object> parameters = new List<object>();

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

            await client.SendAsync("Scan", parameters).ConfigureAwait(false);
        }

        public async static Task SendTTLAsync(this RedisClient client, string key)
        {
            await client.SendAsync("TTL", key).ConfigureAwait(false);
        }

        public async static Task SendTypeAsync(this RedisClient client, string key)
        {
            await client.SendAsync("Type", key).ConfigureAwait(false);
        }
    }
}
