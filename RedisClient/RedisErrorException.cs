using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis
{
    public class RedisErrorException : ApplicationException
    {
        public RedisErrorException(string message)
            : base(message)
        { }
    }

    public class RedisInvalidResponseType : RedisErrorException
    {
        public RedisInvalidResponseType(Type expected, Type actual)
            : base(string.Format("Expected {0} but response was {1}", expected.Name, actual.Name))
        { }
    }
        public class RedisUnexpectedResponse : RedisErrorException
    {
        public RedisUnexpectedResponse(string expected, string actual)
            : base(string.Format("Expected {0} but response was {1}", expected, actual))
        { }

    }
}
