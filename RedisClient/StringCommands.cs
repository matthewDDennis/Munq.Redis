using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public static class StringCommands
    {
        public async static Task SendAppend(this RedisClient client, string key, string value)
        {
            client.SendCommand("Append", key, value);
        }

    }
}
