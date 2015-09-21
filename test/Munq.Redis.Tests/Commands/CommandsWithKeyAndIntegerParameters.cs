using System;
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

        readonly Func<RedisClient, string, int, Task>[] Methods = {
            Keys.Commands.SendExpireAsync,
            Keys.Commands.SendMoveAsync
        };
    }
}
