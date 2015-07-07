using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis
{
    public class RedisException : Exception
    {
        public RedisException(string message)
            : base(message)
        { }
    }

    public class RedisInvalidResponseType : RedisException
    {
        public RedisInvalidResponseType(Type expected, Type actual)
            : base(string.Format("Expected {0} but response was {1}", expected.Name, actual.Name))
        { }
    }
        public class RedisUnexpectedResponse : RedisException
    {
        public RedisUnexpectedResponse(string expected, string actual)
            : base(string.Format("Expected {0} but response was {1}", expected, actual))
        { }

    }
}
