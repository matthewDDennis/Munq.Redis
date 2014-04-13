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
}
