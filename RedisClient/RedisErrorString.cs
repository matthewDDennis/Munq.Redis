using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis
{
    public class RedisErrorString
    {
        public RedisErrorString(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override string ToString()
        {
            return "Redis Error: " + Message;
        }
    }

    public class RedisNull
    {
        public override string ToString()
        {
            return "<null>";
        }
    }

    public class RedisBulkString
    {
        public RedisBulkString(char[] data, int numChars)
        {
            Value = new String(data, 0, numChars);
        }
        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }
    }
}