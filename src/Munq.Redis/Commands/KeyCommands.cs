using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Keys
{
    /// <summary>
    /// Sends the Redis Key Commands as documented at http://redis.io/commands#generic
    /// </summary>
    /// <remarks>
    /// These methods do not handle the response, only send the commands asynchronously.
    /// </remarks>
    public static class Commands
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
            if (keys == null || keys.Count() == 0 || keys.Any(s => string.IsNullOrWhiteSpace(s)))
                throw new ArgumentNullException(nameof(keys));

            return client.SendAsync("Del", keys);
        }

        /// <summary>
        /// Sends the command to Dump the value of a Redis Key. http://redis.io/commands/dump
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendDumpAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

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
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Exists", key);
        }

        /// <summary>
        /// Sends the command to expire a Key after the specified number of seconds. http://redis.io/commands/expire
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="seconds">The number of seconds after which the Key expires.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendExpireAsync(this RedisClient client, string key, int seconds)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Expire", key, seconds);
        }

        /// <summary>
        /// Sends the command to expire a Key at the specified date/time. http://redis.io/commands/expireat
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="unixTimeStamp">The unix timestamp for the date/time to expire the Key.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendEpireAtAsync(this RedisClient client, string key, long unixTimeStamp)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

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
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentNullException(nameof(pattern));

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
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

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
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Move", key, database);
        }

        /// <summary>
        /// Sends the Object Encoding command. http://redis.io/commands/object
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendObjectEncodingAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Object", "Encoding", key);
        }

        /// <summary>
        /// Sends the Object IdleTime command. http://redis.io/commands/object
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendObjectIdelTimeAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Object", "IdeTime", key);
        }

        /// <summary>
        /// Sends the Object RefCount command. http://redis.io/commands/object
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendObjectRefCountAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Object", "RefCount", key);
        }

        /// <summary>
        /// Sends the Persist command. http://redis.io/commands/persist
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendPersistAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Persist", key);
        }

        /// <summary>
        /// Sends the PExpire command. http://redis.io/commands/pexpire
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="milliseconds">The time to live for the Key.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendPExpireAsync(this RedisClient client, string key, long milliseconds)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("PExpire", key, milliseconds);
        }

        /// <summary>
        /// Sends the PExpireAt command. http://redis.io/commands/pexpireat
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="millisecondsTimestamp">The unix timestamp of the date/time to expire the Key.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendPExpireAtAsync(this RedisClient client, string key, long millisecondsTimestamp)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("PExpireAt", key, millisecondsTimestamp);
        }

        /// <summary>
        /// Sends the PExpire command. http://redis.io/commands/pttl
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="milliseconds">The time to live for the Key.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendPTTLAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("PTTL", key);
        }

        /// <summary>
        /// Sends the RandomKey command. http://redis.io/commands/randomkey
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendRandomKeyAsync(this RedisClient client)
        {
            return client.SendAsync("RandomKey");
        }

        /// <summary>
        /// Sends the Rename command. http://redis.io/commands/rename
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="newKey">The new Key name.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendRenameAsync(this RedisClient client, string key, string newKey)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(newKey))
                throw new ArgumentNullException(nameof(newKey));

            return client.SendAsync("Rename", key, newKey);
        }

        /// <summary>
        /// Sends the RenameNX command. http://redis.io/commands/renamenx
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="newKey">The new Key name.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendRenameXAsync(this RedisClient client, string key, string newKey)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(newKey))
                throw new ArgumentNullException(nameof(newKey));

            return client.SendAsync("RenameX", key, newKey);
        }

        /// <summary>
        /// Sends the Restore command. http://redis.io/commands/restore
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="ttl">The expiry time in milliseconds.</param>
        /// <param name="serializeValue">The value to restore from a Dump command.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendRestoreAsync(this RedisClient client, string key, long ttl, byte[] serializeValue)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (serializeValue == null)
                throw new ArgumentNullException(nameof(serializeValue));

            return client.SendAsync("Restore", key, ttl, serializeValue);
        }

        /// <summary>
        /// Sends the Scan command. http://redis.io/commands/scan
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="cursor">The current cursor.</param>
        /// <param name="pattern">The pattern to match the keys against. Null for any key.</param>
        /// <param name="count">The max number of keys to return. Null for the default (10).</param>
        /// <returns></returns>
        public static Task SendScanAsync(this RedisClient client, long cursor, string pattern, long? count)
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

        /// <summary>
        /// Sends the TTL command. http://redis.io/commands/ttl
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendTTLAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("TTL", key);
        }

        /// <summary>
        /// Sends the Type command. http://redis.io/commands/type
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendTypeAsync(this RedisClient client, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Type", key);
        }

        // TODO: change options string to typed parameters or a options class.
        /// <summary>
        /// Sends the Type command. http://redis.io/commands/type
        /// </summary>
        /// <param name="client">The RedisClient.</param>
        /// <param name="key">The key string.</param>
        /// <param name="options">The optional part of the command.  To be removed.</param>
        /// <returns>The task which completes when the command is sent.</returns>
        public static Task SendSortAsync(this RedisClient client, string key, string options)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return client.SendAsync("Sort", key, options);
        }
    }
}
