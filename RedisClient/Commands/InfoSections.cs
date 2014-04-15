using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public enum InfoSections
    {
        Default,
        All,
        Server,
        Clients,
        Memory,
        Persistence,
        Stats,
        Replication,
        CPU,
        CommandStats,
        Cluster,
        Keyspace
    }
}
