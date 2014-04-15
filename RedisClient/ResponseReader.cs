using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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

        public bool HasDataAvailable { get { return _reader.Peek() != -1; } }

        public async Task<object> ReadAsync()
        {
            int c = _reader.Read();
            if (c == -1)
                return null;

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
                    return new RedisErrorString("Invalid response initial character " + (char)c);
            }
        }
        public void Dispose()
        {
            if (_reader != null)
                _reader.Dispose();
        }

        private async Task<string> ReadSimpleStringAsync()
        {
            return await _reader.ReadLineAsync().ConfigureAwait(false);
        }
        private async Task<object> ReadErrorStringAsync()
        {
            string message = await _reader.ReadLineAsync().ConfigureAwait(false);
            return new RedisErrorString(message);
        }
        private async Task<object> ReadLongAsync()
        {
            string intStr = await _reader.ReadLineAsync().ConfigureAwait(false);
            long value;
            if (long.TryParse(intStr, out value))
                return value;
            else
                return new RedisErrorString("Invalid Integer " + intStr);
        }
        private async Task<object> ReadBulkStringAsync()
        {
            object strLenObj = await ReadLongAsync().ConfigureAwait(false);
            if (strLenObj is long)
            {
                long strSize = (long)strLenObj;
                if (strSize == -1)
                    return new RedisNull();
                else
                {
                    char[] chars = new char[strSize];
                    int charsRead = await _reader.ReadBlockAsync(chars, 0, (int)strSize).ConfigureAwait(false);

                    _reader.ReadLine();
                    if (strSize != charsRead)
                        return new RedisErrorString("String length is incorrect. Expecting " + strSize + " received "+ charsRead);
                    else
                        return new string(chars, 0, charsRead);
                }
            }
            else
                return strLenObj;
        }
        private async Task<object> ReadArrayAsync()
        {
            object strLenObj = await ReadLongAsync().ConfigureAwait(false);
            if (strLenObj is long)
            {
                long arraySize = (long)strLenObj;
                if (arraySize == -1)
                    return null;

                object[] results = new object[arraySize];
                for (long i = 0; i < arraySize; i++)
                    results[i] = await ReadAsync().ConfigureAwait(false);
                return results;
            }
            else
                return strLenObj;
        }
    }
}
