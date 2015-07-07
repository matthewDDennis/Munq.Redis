using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Responses
{
    public static class BulkStringResponse
    {
        public static async Task<string> ExpectBulkStringAsync(this RedisClient client)
        {
            object response = await client.ReadResponseAsync().ConfigureAwait(false);
            if (response is RedisBulkString)
                return response.ToString();
            else if (response is RedisNull)
                return null;
            else if (response is RedisErrorString)
            {
                RedisErrorString errorString = response as RedisErrorString;
                throw new RedisException(errorString.Message);
            }
            else
                throw new RedisInvalidResponseType(typeof(RedisBulkString), response.GetType());
        }
    }
}
