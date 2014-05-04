using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public enum StringSetCommandOptions
    {
        Always,
        IfExists,
        IfNotExists
    }
}
