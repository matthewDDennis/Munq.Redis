using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis.Tests.Commands
{
    public class CommandsWithKeyAndIntegerParameters
    {
        static readonly Encoding encoder = new UTF8Encoding();

        public enum Command
        {
            // Key Commands
            Expire,
            Move,

        }

        Func<RedisClient, string, string, Task>[] Methods = {
        };
    }
}
