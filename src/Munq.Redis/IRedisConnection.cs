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
        Task ReconnectAsync();
        void Close();

        Stream GetStream();
    }
}
