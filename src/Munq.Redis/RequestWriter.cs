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
    public class RequestWriter
    {
        static readonly byte[] CRLF       = { (byte)'\r', (byte)'\n' };
        static readonly byte[] NullString = { (byte)'$', (byte)'-', (byte)'1', (byte)'\r', (byte)'\n' };

        readonly Encoding      _encoder   = new UTF8Encoding();
        readonly Stream        _stream;

        public RequestWriter(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Sends a command and it's parameters to the Stream.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The paramaters for the command.</param>
        /// <returns>The bytes to send to the Redis Server.</returns>
        public  async Task WriteRedisCommandAsync(string command, IEnumerable<object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentNullException(nameof(command));

            var sizeOfCommandArray = 1 + (parameters?.Count() ?? 0);
            await WriteStringToStreamAsync($"*{sizeOfCommandArray}\r\n").ConfigureAwait(false);

            await WriteRedisBulkStringAsync(command).ConfigureAwait(false);

            if (sizeOfCommandArray > 1)
            {
                foreach (object obj in parameters)
                    await WriteObjectAsync(obj).ConfigureAwait(false);
            }
            await _stream.FlushAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Writes an object to the Stream.
        /// </summary>
        /// <param name="value">The object to add.</param>
        /// <returns>An awaitable task that completes when the object has been written to the stream.</returns>
        Task WriteObjectAsync(object value)
        {
            if (value == null)
                return WriteBytesToStreamAsync(NullString);

            var objType = value.GetType();

            if (objType == typeof(string))
                return WriteRedisBulkStringAsync(value as string);
            else if (objType == typeof(byte[]))
                return WriteRedisBulkStringAsync(value as byte[]);
            else if (objType == typeof(bool))
                return WriteRedisBulkStringAsync((bool)value ? "1" : "0");
            else
                return WriteRedisBulkStringAsync(value.ToString());
        }

        /// <summary>
        /// Writes a string as a RedisBulkString to the Stream.
        /// </summary>
        /// <param name="str">The string to write.</param>
        /// <returns>An awaitable task that completes when the string has been written to the stream.</returns>
        Task WriteRedisBulkStringAsync(string str)
        {
            byte[] data = (str != null) ? _encoder.GetBytes(str) : null;
            return WriteRedisBulkStringAsync(data);
        }

        /// <summary>
        /// Writes an array of bytes as a RedisBulkString to the Stream.
        /// </summary>
        /// <param name="data">The bytes to write.</param>
        /// <returns>An awaitable task that completes when the byte[] has been written to the stream.</returns>
        async Task WriteRedisBulkStringAsync(byte[] data)
        {
            if (data != null)
            {
                await WriteStringToStreamAsync($"${data.Length}\r\n").ConfigureAwait(false);
                await WriteBytesToStreamAsync(data).ConfigureAwait(false);
                await WriteBytesToStreamAsync(CRLF).ConfigureAwait(false);
            }
            else
            {
                await WriteNullToStreamAsync();
            }
        }

        /// <summary>
        /// Converts the string to a byte[] and writes it to the stream.
        /// </summary>
        /// <param name="redisString">The string to wriet.</param>
        /// <returns>An awaitable task that completes when the string has been written to the stream.</returns>
        Task WriteStringToStreamAsync(string redisString)
        {
            byte[] data = _encoder.GetBytes(redisString);
            return WriteBytesToStreamAsync(data);
        }

        /// <summary>
        /// Writes the NullString to the stream.
        /// </summary>
        /// <returns>An awaitable task that completes when the Null string has been written to the stream.</returns>
        Task WriteNullToStreamAsync()
        {
            return WriteBytesToStreamAsync(NullString);
        }

        /// <summary>
        /// Writes a byte array to the stream.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>An awaitable task that completes when the byte[] has been written to the stream.</returns>
        Task WriteBytesToStreamAsync(byte[] data)
        {
            return _stream.WriteAsync(data, 0, data.Length);
        }
    }
}
