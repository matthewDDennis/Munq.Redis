using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Munq.Redis;

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

        readonly Func<RedisClient, string, int, Task>[] Methods = {
            Redis.Commands.KeyCommands.SendExpireAsync,
            Redis.Commands.KeyCommands.SendMoveAsync
        };
    }
}
