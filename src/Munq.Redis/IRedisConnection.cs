using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public interface IRedisConnection : IDisposable
    {
        bool IsConnected { get; }
        int Database { get; }

        Task ConnectAsync();
        void Close();

        Stream GetStream();
    }
}
