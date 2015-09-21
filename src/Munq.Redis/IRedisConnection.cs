using System;
using System.IO;
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
