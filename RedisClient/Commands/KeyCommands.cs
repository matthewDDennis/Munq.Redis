using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class KeyCommands
    {
        public async static Task SendDeleteCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Del", key).ConfigureAwait(false);
        }

        public async static Task SendDeleteCommandAsync(this RedisClient client, 
                                                        IEnumerable<string> keys)
        {
            await client.SendCommandAsync("Del", (keys ?? new string[]{}).ToArray())
                        .ConfigureAwait(false);
        }

        public async static Task SendDumpCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Dump", key).ConfigureAwait(false);
        }

        public async static Task SendExistsCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Exists", key).ConfigureAwait(false);
        }

        public async static Task SendExpireCommandAsync(this RedisClient client, string key, 
                                                        int seconds)
        {
            await client.SendCommandAsync("Expire", key, seconds).ConfigureAwait(false);
        }

        public async static Task SendEpireAtCommandAsync(this RedisClient client, string key, 
                                                         long unixTimeStamp)
        {
            await client.SendCommandAsync("ExpireAt", key, unixTimeStamp).ConfigureAwait(false);
        }

        public async static Task SendKeysCommandAsync(this RedisClient client, string pattern)
        {
            await client.SendCommandAsync("Keys", pattern).ConfigureAwait(false);
        }

        public async static Task SendMigrateCommandAsync(this RedisClient client, string host, 
                                                         int port, string key, int destination_db, 
                                                         long timeoutMs)
        {
            await client.SendCommandAsync("Migrate", host, port, key, destination_db, timeoutMs)
                        .ConfigureAwait(false);
        }

        public async static Task SendMoveCommandAsync(this RedisClient client, string key, 
                                                      int database)
        {
            await client.SendCommandAsync("Move", key, database).ConfigureAwait(false);
        }

        public async static Task SendObjectCommandAsync(this RedisClient client, string subcommand, 
                                                        string key)
        {
            await client.SendCommandAsync("Object", subcommand, key).ConfigureAwait(false);
        }

        public async static Task SendPersistCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Persist", key).ConfigureAwait(false);
        }

        public async static Task SendPExpireCommandAsync(this RedisClient client, string key, 
                                                         long milliseconds)
        {
            await client.SendCommandAsync("PExpire", key, milliseconds).ConfigureAwait(false);
        }

        public async static Task SendPExpireAtCommandAsync(this RedisClient client, string key, 
                                                           long millisecondsTimestamp)
        {
            await client.SendCommandAsync("PExpireAt", key, millisecondsTimestamp)
                        .ConfigureAwait(false);
        }

        public async static Task SendPTTLCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("PTTL", key).ConfigureAwait(false);
        }

        public async static Task SendRandomKeyCommandAsync(this RedisClient client)
        {
            await client.SendCommandAsync("RandomKey").ConfigureAwait(false);
        }

        public async static Task SendRenameCommandAsync(this RedisClient client, string key, 
                                                        string newKey)
        {
            await client.SendCommandAsync("Rename", key, newKey).ConfigureAwait(false);
        }

        public async static Task SendRenameXCommandAsync(this RedisClient client, string key, 
                                                         string newKey)
        {
            await client.SendCommandAsync("RenameX", key, newKey).ConfigureAwait(false);
        }

        public async static Task SendRestoreCommandAsync(this RedisClient client, string key, 
                                                         long ttl, string serializeValue)
        {
            await client.SendCommandAsync("Restore", key, ttl, serializeValue)
                        .ConfigureAwait(false);
        }

        // TODO: finish this one.  Its a little more complicated than most.
        //public async static Task SenSort(this RedisClient client, string key, ...)
        //{
        //    await client.SendCommand("Sort", key, ...).ConfigureAwait(false);
        //}

        public async static Task SendScanCommandAsync(this RedisClient client, long cursor, 
                                                      string pattern, long? count)
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

            await client.SendCommandAsync("Scan", parameters).ConfigureAwait(false);
        }

        public async static Task SendTTLCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("TTL", key).ConfigureAwait(false);
        }

        public async static Task SendTypeCommandAsync(this RedisClient client, string key)
        {
            await client.SendCommandAsync("Type", key).ConfigureAwait(false);
        }
    }
}
