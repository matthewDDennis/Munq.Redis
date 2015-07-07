﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Commands
{
    /// <summary>
    /// Sends the Redis Key Commands as documented at http://redis.io/commands#generic
    /// </summary>
    /// <remarks>
    /// These methods do not handle the response, only send the commands asynchronously.
    /// </remarks>
    public static class KeyCommands
    {
        /// <summary>
        /// Sends the command to delete one or more Redis Keys. http://redis.io/commands/del
        /// </summary>
        /// <example>
        /// client.SendDeleteAsync("key1", "key2");
        /// </example>
        /// <param name="client">The RedisClient.</param>
        /// <param name="keys">The paramater array of one or more key strings.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendDeleteAsync(this RedisClient client, params string[] keys)
        {
            return client.SendDeleteAsync((IEnumerable<string>)keys);
        }

        /// <summary>
        /// Sends the command to delete one or more Redis Keys. http://redis.io/commands/del
        /// </summary>
        /// <example>
        /// var keys = new string[] {"key1", "key2"};
        /// client.SendDeleteAsync(keys);
        /// </example>
        /// <param name="client">The RedisClient.</param>
        /// <param name="keys">The IEnumerable of one or more key strings.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendDeleteAsync(this RedisClient client, IEnumerable<string> keys)
        {
            return client.SendAsync("Del", (keys ?? new string[] { }));
        }

        /// <summary>
        /// Sends the command to Dump the value of a Redis Key. http://redis.io/commands/dump
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendDumpAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Dump", key);
        }

        /// <summary>
        /// Sends the command to check if a key exists. http://redis.io/commands/exists
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendExistsAsync(this RedisClient client, string key)
        {
            return client.SendAsync("Exists", key);
        }

        /// <summary>
        /// Sends the command to expire a Key after the specified number of seconds. http://redis.io/commands/expire
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="seconds">The number of seconds after which the Key expires.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendExpireAsync(this RedisClient client,
                                                 string key, int seconds)
        {
            return client.SendAsync("Expire", key, seconds);
        }

        /// <summary>
        /// Sends the command to expire a Key at the specified date/time. http://redis.io/commands/expireat
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="unixTimeStamp">The unix timestamp for the date/time to expire the Key.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendEpireAtAsync(this RedisClient client,
                                                  string key, long unixTimeStamp)
        {
            return client.SendAsync("ExpireAt", key, unixTimeStamp);
        }

        /// <summary>
        /// Sends the command to get the keys matching a pattern. http://redis.io/commands/keys
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendKeysAsync(this RedisClient client, string pattern)
        {
            return client.SendAsync("Keys", pattern);
        }

        /// <summary>
        /// Sends the command to move a Key from the current database to another, possibly on another Redis instance. 
        /// http://redis.io/commands/migrate
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="host">The host address of the destination Redis instance.</param>
        /// <param name="port">The port number of the destination Redis instance.</param>
        /// <param name="key">The key string.</param>
        /// <param name="destination_db">The integer identifier of the destination database.</param>
        /// <param name="timeoutMs">The timeout, in milliseconds, for th completion of the operation.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendMigrateAsync(this RedisClient client, string host,
                                                  int port, string key, int destination_db,
                                                  long timeoutMs)
        {
            return client.SendAsync("Migrate", host, port, key, destination_db, timeoutMs);
        }

        /// <summary>
        /// Sends the command to Move a Key to a different database. http://redis.io/commands/move
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="database">The integer identifier of the destination database.</param>
        /// <returns>The task which completes when the command is sent.</returns>
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
            return client.SendAsync("PExpireAt", key, millisecondsTimestamp);
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
                                                  string key, long ttl, byte[] serializeValue)
        {
            return client.SendAsync("Restore", key, ttl, serializeValue);
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

        // TODO: change options string to typed parameters or a options class.
        public static Task SendSortAsync(this RedisClient client, string key, string options)
        {
            return client.SendAsync("Sort", key, options);
        }
    }
}
