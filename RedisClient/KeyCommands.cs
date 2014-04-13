using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class KeyCommands
    {
        public async static Task SendDelete(this RedisClient client, string key)
        {
            await client.SendCommand("Del", key);
        }

        public async static Task SendDelete(this RedisClient client, IEnumerable<string> keys)
        {
            await client.SendCommand("Del", keys.ToArray());
        }

        public async static Task SendDump(this RedisClient client, string key)
        {
            await client.SendCommand("Dump", key);
        }

        public async static Task SendExists(this RedisClient client, string key)
        {
            await client.SendCommand("Exists", key );
        }

        public async static Task SendExpire(this RedisClient client, string key, int seconds)
        {
            await client.SendCommand("Expire", key, seconds);
        }

        public async static Task SendEpireAt(this RedisClient client, string key, long unixTimeStamp)
        {
            await client.SendCommand("ExpireAt", key, unixTimeStamp);
        }

        public async static Task SendKeys(this RedisClient client, string pattern)
        {
            await client.SendCommand("Keys", pattern);
        }

        public async static Task SendMigrate(this RedisClient client, string host, int port, 
                                             string key, int destination_db, long timeoutMs)
        {
            await client.SendCommand("Migrate", host, port, key, destination_db, timeoutMs);
        }

        public async static Task SendMove(this RedisClient client, string key, int database)
        {
            await client.SendCommand("Move", key, database);
        }

        public async static Task SendObject(this RedisClient client, string subcommand, string key)
        {
            await client.SendCommand("Object", subcommand, key);
        }

        public async static Task SendPersist(this RedisClient client, string key)
        {
            await client.SendCommand("Persist",  key);
        }

        public async static Task SendPExpire(this RedisClient client, string key, long milliseconds)
        {
            await client.SendCommand("PExpire", key, milliseconds);
        }

        public async static Task SendPExpireAt(this RedisClient client, string key, long millisecondsTimestamp)
        {
            await client.SendCommand("PExpireAt", key, millisecondsTimestamp);
        }

        public async static Task SendPTTL(this RedisClient client, string key)
        {
            await client.SendCommand("PTTL", key);
        }

        public async static Task SendRandomKey(this RedisClient client)
        {
            await client.SendCommand("RandomKey");
        }

        public async static Task SendRename(this RedisClient client, string key, string newKey)
        {
            await client.SendCommand("Rename", key, newKey);
        }

        public async static Task SendRenameX(this RedisClient client, string key, string newKey)
        {
            await client.SendCommand("RenameX", key, newKey);
        }
        public async static Task SendRestore(this RedisClient client, string key, long ttl, string serializeValue)
        {
            await client.SendCommand("Restore", key, ttl, serializeValue);
        }

        // TODO: finish this one.  Its a little more complicated than most.
        //public async static Task SenSort(this RedisClient client, string key, ...)
        //{
        //    await client.SendCommand("Sort", key, ...);
        //}

        public async static Task SendTTL(this RedisClient client, string key)
        {
            await client.SendCommand("TTL", key);
        }

        public async static Task SendType(this RedisClient client, string key)
        {
            await client.SendCommand("Type", key);
        }

        public async static Task SendScan(this RedisClient client, long cursor, string pattern, long? count)
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

            await client.SendCommand("Scan", parameters);
        }



    }
}
