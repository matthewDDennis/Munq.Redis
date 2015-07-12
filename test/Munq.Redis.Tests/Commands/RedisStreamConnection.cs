using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Munq.Redis.Tests.Commands
{
    public class RedisStreamConnection : IRedisConnection
    {
        Stream _stream;

        public RedisStreamConnection(Stream stream, int database = 0)
        {
            _stream  = stream;
            Database = database;
        }

        public int Database { get; }

        public bool IsConnected => _stream != null;

        public void Close()
        {
            Dispose();
        }

        public Task ConnectAsync()
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
        }

        public Stream GetStream()
        {
            return _stream;
        }
    }
}
