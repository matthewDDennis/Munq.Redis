using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis
{
    public enum StringSetCommandOptions
    {
        Always,
        IfExists,
        IfNotExists
    }
}
