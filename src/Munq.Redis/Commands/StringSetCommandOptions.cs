using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis.Commands
{
    public enum StringSetCommandOptions
    {
        Always,
        IfExists,
        IfNotExists
    }
}
