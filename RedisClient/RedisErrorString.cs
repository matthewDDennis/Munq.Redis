using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public RedisBulkString(byte[] data)
        {
            Value = data;
        }

        public RedisBulkString(string data)
        {
            Value = Encoding.UTF8.GetBytes(data);
        }

        public byte[] Value { get; private set; }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Value);
        }
    }
}