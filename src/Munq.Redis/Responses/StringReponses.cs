using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Responses
{
    public static class StringReponses
    {
        const string OkResponse = "OK";
        public static async Task<string> ExpectStringAsync(this RedisClient client)
        {
            var response = await client.ReadResponseAsync().ConfigureAwait(false);
            if (response is string)
                return response as string;
            else if (response is RedisErrorString)
            {
                var errorString = response as RedisErrorString;
                throw new RedisException(errorString.Message);
            }
            else
                throw new RedisInvalidResponseType(typeof(string), response.GetType());
        }

        public static async Task<bool> ExpectConstStringAsync(this RedisClient client, string expected)
        {
            string response = await client.ExpectStringAsync().ConfigureAwait(false);
            if (string.Compare(response, expected) != 0)
                throw new RedisUnexpectedResponse(expected, response);
            else
                return true;
        }

        public static Task<bool> ExpectOkAsync(this RedisClient client)
        {
            return client.ExpectConstStringAsync(OkResponse);
        }
    }
}
