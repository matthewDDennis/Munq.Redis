using System;
using System.Collections.Generic;
using System.Linq;

namespace Munq.Redis
{
    public enum InfoSections
    {
        All,
        Clients,
        Cluster,
        CommandStats,
        CPU,
        Default,
        Keyspace,
        Memory,
        Persistence,
        Replication,
        Server,
        Stats
    }
}
