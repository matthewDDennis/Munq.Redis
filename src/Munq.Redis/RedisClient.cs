using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisClient : IDisposable
    {
        Stream                     _stream;
        ResponseReader             _responseReader;
        CommandWriter              _commandWriter;

        public RedisClient(Stream stream)
        {
            _stream         = stream;
            _responseReader = new ResponseReader(_stream);
            _commandWriter  = new CommandWriter(_stream);
        }

        /// <summary>
        /// Gets the response reader.
        /// </summary>
        /// <returns>The response read.</returns>
        public async Task<object> ReadResponseAsync()
        {
            try
            {
                return await _responseReader.ReadRedisResponseAsync();
            }
            catch (Exception ex)
            {
                return new RedisErrorString("ERR - " + ex.Message);
            }
        }

        /// <summary>
        /// Sends a command and returns the response string.
        /// </summary>
        /// <param name="command">The Command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The response.</returns>
        public Task SendAsync(string command, params object[] parameters)
        {
            return SendAsync(command, (IEnumerable<object>)parameters);
        }

        /// <summary>
        /// Sends a command and returns the response string.
        /// </summary>
        /// <param name="command">The Command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The awaitable Task.</returns>
        public async Task SendAsync(string command, IEnumerable<object> parameters = null)
        {
            await _commandWriter.WriteRedisCommandAsync(command, parameters).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
