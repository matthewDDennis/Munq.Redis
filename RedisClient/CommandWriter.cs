using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    /// <summary>
    /// The command builder is a static class which is used to build Redis Server commands from and command
    /// name and a collection of parameters.
    /// </summary>
    /// <remarks>This class is safe for concurrent, multi-thread access.</remarks>
    public static class CommandWriter
    {
        static Encoding encoder = new UTF8Encoding();
        static byte[] crlf = { (byte)'\r', (byte)'\n' };

        /// <summary>
        /// Builds an array of bytes to send to the Redis Server for the command and it's parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The paramaters for the command.</param>
        /// <returns>The bytes to send to the Redis Server.</returns>
        public static async Task WriteRedisCommandAsync(this Stream stream, string command, IEnumerable<object> parameters = null)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentNullException("command");

            var sizeOfCommandArray = 1 + (parameters != null ? parameters.Count() : 0);
            var redisString = string.Format("*{0}\r\n", sizeOfCommandArray);
            byte[] bytes = encoder.GetBytes(redisString);
            await stream.WriteAsync(bytes, 0, bytes.Length);

            await WriteRedisBulkStringAsync(stream, command);

            if (sizeOfCommandArray > 1)
            {
                foreach (object obj in parameters)
                    await WriteObjectAsync(stream, obj);
            }
        }

        /// <summary>
        /// Adds an object to the command data.
        /// </summary>
        /// <param name="value">The object to add.</param>
        static async Task WriteObjectAsync(Stream stream, object value)
        {
            var objType = value.GetType();

            if (objType == typeof(string))
                await WriteRedisBulkStringAsync(stream, value as string);
            else if (objType == typeof(byte[]))
                await WriteRedisBulkStringAsync(stream, value as byte[]);
            else
            {
                if (objType == typeof(bool))
                {
                    value = (bool)value ? "1" : "0";
                }
                await WriteRedisBulkStringAsync(stream, value.ToString());
            }
        }

        /// <summary>
        /// Writes a string as a RedisBulkString to the Stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="str">The string to write.</param>
        static async Task WriteRedisBulkStringAsync(Stream stream, string str)
        {
            string redisString;
            byte[] data;
            if (str != null)
                redisString = string.Format("${0}\r\n{1}\r\n", str.Length, str);
            else
                redisString = "$-1\r\n";

            data = encoder.GetBytes(redisString);
            await stream.WriteAsync(data, 0, data.Length);
        }

        /// <summary>
        /// Writes an array of bytes as a RedisBulkString to the Stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="data">The bytes to write.</param>
        static async Task WriteRedisBulkStringAsync(Stream stream, byte[] data)
        {
            string redisString;
            byte[] bytes;
            if (data != null)
            {
                redisString = string.Format("${0}\r\n", data.Length);
                bytes = encoder.GetBytes(redisString);
                await stream.WriteAsync(bytes, 0, bytes.Length);
                await stream.WriteAsync(data, 0, data.Length);
                await stream.WriteAsync(crlf, 0, crlf.Length);
            }
            else
            {
                bytes = encoder.GetBytes("$-1\r\n");
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
