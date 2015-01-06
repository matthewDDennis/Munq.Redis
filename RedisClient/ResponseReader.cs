using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Munq.Redis
{
    /// <summary>
    /// The ResponseParser class parses a string or Stream and converts the Redis response
    /// into a object with the response data either as a single value or an array of values.
    /// </summary>
    public class ResponseReader : IDisposable
    {
        NetworkStream _reader;

        /// <summary>
        /// Initializes a new instance of the ResponseReader class from a Stream.
        /// </summary>
        /// <param name="stream">The Stream.</param>
        public ResponseReader(NetworkStream stream)
        {
            _reader = stream;
        }

        /// <summary>
        /// Reads and parses the response from the ResponseReader's string or stream.
        /// </summary>
        /// <returns>A task which returns an object created from the response data.</returns>
        public async Task<object> ReadAsync()
        {
            var buffer = new byte[1];
            var count = await _reader.ReadAsync(buffer, 0, 1).ConfigureAwait(false);
            if (count == 0)
            {
                return null;
            }
            var c = (char)buffer[0];
            switch (c)
            {
                case '+':
                    return await ReadSimpleStringAsync().ConfigureAwait(false);

                case '-':
                    return await ReadErrorStringAsync().ConfigureAwait(false);

                case ':':
                    return await ReadLongAsync().ConfigureAwait(false);

                case '$':
                    return await ReadBulkStringAsync().ConfigureAwait(false);

                case '*':
                    return await ReadArrayAsync().ConfigureAwait(false);

                default:
                    return new RedisErrorString("Invalid response initial character " + c);
            }
        }

        /// <summary>
        /// Disposes of the internal TextReader.
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }

        /// <summary>
        /// Reads an array of values from the response data.
        /// </summary>
        /// <returns></returns>
        async Task<object> ReadArrayAsync()
        {
            var strLenObj = await ReadLongAsync().ConfigureAwait(false);
            if (strLenObj is long)
            {
                var arraySize = (long)strLenObj;
                if (arraySize == -1)
                {
                    return null;
                }
                var results = new object[arraySize];
                for (var i = 0L; i < arraySize; i++)
                {
                    results[i] = await ReadAsync().ConfigureAwait(false);
                }
                return results;
            }
            else
            {
                return strLenObj;
            }
        }

        /// <summary>
        /// Reads a BulkString from the response data.
        /// </summary>
        /// <returns>Returns the BulkString, a RedisNull for the BulkString of length -1, or and RedisErrorString.</returns>
        async Task<object> ReadBulkStringAsync()
        {
            var strLenObj = await ReadLongAsync().ConfigureAwait(false);
            if (strLenObj is long)
            {
                var strSize = (long)strLenObj;
                if (strSize == -1)
                {
                    return new RedisNull();
                }
                else
                {
                    var data = new byte[strSize];
                    int charsRead = 0;
                    if (strSize > 0)
                        charsRead = await _reader.ReadAsync(data, 0, (int)strSize).ConfigureAwait(false);

                    string remainingCharacters = await ReadLineAsync().ConfigureAwait(false);
                    if (strSize != charsRead || remainingCharacters.Length != 0)
                    {
                        // TASK: Should be an exception.
                        return new RedisErrorString(String.Format(
                            "String length is incorrect. Expecting {0} received {1} and {2} extra characters.",
                            strSize, charsRead, remainingCharacters.Length));
                    }
                    else
                    {
                        return new RedisBulkString(data);
                    }
                }
            }
            else
            {
                return strLenObj;
            }
        }

        /// <summary>
        /// Reads a Redis Server Error from the Response data.
        /// </summary>
        /// <returns>The RedisErrorString containing the error message.</returns>
        async Task<object> ReadErrorStringAsync()
        {
            var message = await ReadLineAsync().ConfigureAwait(false);
            return new RedisErrorString(message);
        }

        /// <summary>
        /// Read line from the stream and converts it to a long.
        /// </summary>
        /// <returns>A task which returns a long.</returns>
        async Task<object> ReadLongAsync()
        {
            var intStr = await ReadLineAsync().ConfigureAwait(false);
            long value;
            if (long.TryParse(intStr, out value))
            {
                return value;
            }
            else
            {
                // should this be an exception?
                return new RedisErrorString("Invalid Integer " + intStr);
            }
        }

        /// <summary>
        /// Reads a crlf terminated string from the response stream.
        /// </summary>
        /// <returns>A task which returns a string result.</returns>
        async Task<string> ReadSimpleStringAsync()
        {
            return await ReadLineAsync().ConfigureAwait(false);
        }

        private async Task<string> ReadLineAsync()
        {
            const byte CR = (byte)'\r';
            const byte LF = (byte)'\n';

            var MS = new MemoryStream();

            byte[] input = new byte[1];
            byte prevInput = 0;

            bool done = false;
            while (!done)
            {
                int count = await _reader.ReadAsync(input, 0, 1);
                byte c = input[0];
               switch (c)
                {
                    case CR:
                        break;

                    case LF:
                        if (prevInput == CR )
                            done = true;
                        else
                            MS.WriteByte(c);
                        break;

                    default:
                        if (prevInput == CR)
                            MS.WriteByte(CR);
                        MS.WriteByte(c);
                        break;
                }
                prevInput = c;
            }

            return Encoding.UTF8.GetString(MS.ToArray());
        }
    }
}
