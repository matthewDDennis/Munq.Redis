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
        readonly IRedisConnection _connection;
        Stream                    _stream;
        ResponseReader            _responseReader;
        CommandWriter             _commandWriter;

        public RedisClient(IRedisConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Gets the response reader.
        /// </summary>
        /// <returns>The response read.</returns>
        public async Task<object> ReadResponseAsync()
        {
            try
            {
                return await _responseReader.ReadRedisResponseAsync().ConfigureAwait(false);
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
            await EnsureConnected().ConfigureAwait(false);
            await _commandWriter.WriteRedisCommandAsync(command, parameters).ConfigureAwait(false);
        }

        async Task EnsureConnected()
        {
            if (!_connection.IsConnected)
            {
                await _connection.ConnectAsync().ConfigureAwait(false);
                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }
            }

            if (_stream == null)
            {
#if DNXCORE50
                _stream = _connection.GetStream();
#else
                Stream networkStream = _connection.GetStream();
                _stream              = new BufferedStream(networkStream);
#endif
                _commandWriter  = new CommandWriter(_stream);
                _responseReader = new ResponseReader(_stream);

                // Not the right place for this?
                //if (_connection.Database != RedisClientConfig.DefaultDatabase)
                //{
                //    await SendAsync("Select", _connection.Database);
                //    await ReadResponseAsync();
                //}
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
