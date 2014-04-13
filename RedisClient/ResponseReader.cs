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
        private readonly StringReader _reader;
        public ResponseReader(string data)
        {
            _reader = new StringReader(data);
        }

        public bool HasDataAvailable { get { return _reader.Peek() != -1; } }

        public object Read()
        {
            int c = _reader.Read();
            if (c == -1)
                return null;

            switch (c)
            {
                case '+':
                    return ReadSimpleString();

                case '-':
                    return ReadErrorString();

                case ':':
                    return ReadLong();

                case '$':
                    return ReadBulkString();

                case '*':
                    return ReadArray();

                default:
                    return new RedisErrorString("Invalid character " + (char)c);
            }
        }
        public void Dispose()
        {
            if (_reader != null)
                _reader.Dispose();
        }

        private object ReadSimpleString()
        {
            return _reader.ReadLine();
        }
        private object ReadErrorString()
        {
            string message = _reader.ReadLine();
            return new RedisErrorString(message);
        }
        private object ReadLong()
        {
            string intStr = _reader.ReadLine();
            long value;
            if (long.TryParse(intStr, out value))
                return value;
            else
                return new RedisErrorString("Invalid Integer " + intStr);
        }
        private object ReadBulkString()
        {
            object strLenObj = ReadLong();
            if (strLenObj is long)
            {
                long strSize = (long)strLenObj;
                if (strSize == -1)
                    return new RedisNull();
                else
                {
                    char[] chars = new char[strSize];
                    int charsRead = _reader.Read(chars, 0, (int)strSize);
                    _reader.ReadLine();
                    if (strSize != charsRead)
                        return new RedisErrorString("String length is incorrect.");
                    else
                        return new string(chars, 0, charsRead);
                }
            }
            else
                return strLenObj;
        }
        private object ReadArray()
        {
            object strLenObj = ReadLong();
            if (strLenObj is long)
            {
                long arraySize = (long)strLenObj;
                if (arraySize == -1)
                    return null;

                object[] results = new object[arraySize];
                for (long i = 0; i < arraySize; i++)
                    results[i] = Read();
                return results;
            }
            else
                return strLenObj;
        }
    }
}
