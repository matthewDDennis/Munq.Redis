using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Munq.Redis
{
    public class ResponseReader : IDisposable
    {
        private readonly TextReader _reader;

        public ResponseReader(string data)
        {
            _reader = new StringReader(data);
        }
        public ResponseReader(Stream stream)
        {
            _reader = new StreamReader(stream);
        }

        public bool HasDataAvailable
        {
            get { return _reader.Peek() != -1; }
        }

        public async Task<object> ReadAsync()
        {
            var buffer = new char[1];
            var count = await _reader.ReadAsync(buffer, 0, 1).ConfigureAwait(false);
            if (count == 0)
            {
                return null;
            }
            var c = buffer[0];
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

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }
        }

        private async Task<object> ReadArrayAsync()
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
        private async Task<object> ReadBulkStringAsync()
        {
            var strLenObj = await ReadLongAsync().ConfigureAwait(false);
            if (strLenObj is long)
            {
                var strSize = (long)strLenObj;
                if (strSize == -1)
                {;
                    return new RedisNull();
                }
                else
                {
                    var chars = new char[strSize];
                    int charsRead = 0;
                    if (strSize > 0)
                        charsRead = await _reader.ReadBlockAsync(chars, 0, (int)strSize).ConfigureAwait(false);

                    string remainingCharacters = _reader.ReadLine();
                    if (strSize != charsRead || remainingCharacters.Length != 0)
                    {
                        return new RedisErrorString(String.Format(
                            "String length is incorrect. Expecting {0} received {1} and {2} extra characters.", 
                            strSize, charsRead, remainingCharacters.Length));
                    }
                    else
                    {
                        return new string(chars, 0, charsRead);
                    }
                }
            }
            else
            {
                return strLenObj;
            }
        }
        private async Task<object> ReadErrorStringAsync()
        {
            var message = await _reader.ReadLineAsync().ConfigureAwait(false);
            return new RedisErrorString(message);
        }

        /// <summary>
        /// Read line from the stream and converts it to a long.
        /// </summary>
        /// <returns>A task which returns a long.</returns>
        private async Task<object> ReadLongAsync()
        {
            var intStr = await _reader.ReadLineAsync().ConfigureAwait(false);
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
        private async Task<string> ReadSimpleStringAsync()
        {
            return await _reader.ReadLineAsync().ConfigureAwait(false);
        }
    }
}
