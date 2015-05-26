using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    /// <summary>
    /// The command builder is used to build Redis Server commands from and command
    /// name and a collection of parameters.
    /// </summary>
    /// <remarks>This class is safe for concurrent, multi-thread access.</remarks>
    public class CommandWriter
    {
        static readonly Encoding encoder = new UTF8Encoding();
        static readonly byte[]   crlf = { (byte)'\r', (byte)'\n' };
        Stream                   _stream;

        public CommandWriter(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Builds an array of bytes to send to the Redis Server for the command and it's parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The paramaters for the command.</param>
        /// <returns>The bytes to send to the Redis Server.</returns>
        public  async Task WriteRedisCommandAsync(string command, IEnumerable<object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentNullException(nameof(command));

            var sizeOfCommandArray = 1 + (parameters != null ? parameters.Count() : 0);
            var redisString = string.Format("*{0}\r\n", sizeOfCommandArray);
            byte[] bytes = encoder.GetBytes(redisString);
            await _stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);

            await WriteRedisBulkStringAsync(command);

            if (sizeOfCommandArray > 1)
            {
                foreach (object obj in parameters)
                    await WriteObjectAsync(obj);
            }
        }

        /// <summary>
        /// Adds an object to the command data.
        /// </summary>
        /// <param name="value">The object to add.</param>
        async Task WriteObjectAsync(object value)
        {
            var objType = value.GetType();

            if (objType == typeof(string))
                await WriteRedisBulkStringAsync(value as string);
            else if (objType == typeof(byte[]))
                await WriteRedisBulkStringAsync(value as byte[]);
            else
            {
                if (objType == typeof(bool))
                {
                    value = (bool)value ? "1" : "0";
                }
                await WriteRedisBulkStringAsync(value.ToString());
            }
        }

        /// <summary>
        /// Writes a string as a RedisBulkString to the Stream.
        /// </summary>
        /// <param name="str">The string to write.</param>
        async Task WriteRedisBulkStringAsync(string str)
        {
            byte[] data;
            string redisString = str != null ? string.Format("${0}\r\n{1}\r\n", str.Length, str) : "$-1\r\n";
            data = encoder.GetBytes(redisString);
            await _stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes an array of bytes as a RedisBulkString to the Stream.
        /// </summary>
        /// <param name="data">The bytes to write.</param>
        async Task WriteRedisBulkStringAsync(byte[] data)
        {
            string redisString;
            byte[] bytes;
            if (data != null)
            {
                redisString = string.Format("${0}\r\n", data.Length);
                bytes = encoder.GetBytes(redisString);
                await _stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
                await _stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                await _stream.WriteAsync(crlf, 0, crlf.Length).ConfigureAwait(false);
            }
            else
            {
                bytes = encoder.GetBytes("$-1\r\n");
                await _stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
            }
        }
    }
}
