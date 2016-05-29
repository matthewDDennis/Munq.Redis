using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis.Requests
{
    public enum StringSetCommandOptions
    {
        Always,
        IfExists,
        IfNotExists
    }
}
