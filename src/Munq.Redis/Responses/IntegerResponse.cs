using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Responses
{
    public static class IntegerResponse
    {
        public static async Task<long> ExpectIntegerAsync(this RedisClient client)
        {
            object response = await client.ReadResponseAsync().ConfigureAwait(false);
            if (response is long)
                return (long)response;
            else if (response is RedisErrorString)
            {
                RedisErrorString errorString = response as RedisErrorString;
                throw new RedisException(errorString.Message);
            }
            else
                throw new RedisInvalidResponseType(typeof(RedisBulkString), response.GetType());
        }

        public static async Task<bool> ExpectBitAsync(this RedisClient client)
        {
            long response = await client.ExpectIntegerAsync().ConfigureAwait(false);
            switch (response)
            {
                case 0: return false;
                case 1: return true;
                default: throw new RedisUnexpectedResponse("0 or 1", response.ToString());
            }
        }
    }
}
