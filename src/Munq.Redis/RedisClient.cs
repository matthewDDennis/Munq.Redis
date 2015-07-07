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
        readonly RedisClientConfig _config;
        readonly TcpClient         _tcpClient;
        Stream                     _stream;
        ResponseReader             _responseReader;
        CommandWriter              _commandWriter;

        public RedisClient() : this(new RedisClientConfig())
        {
        }

        public RedisClient(RedisClientConfig config)
        {
            _config         = config;
            _tcpClient      = new TcpClient() 
            { 
                ReceiveTimeout = _config.ReceiveTimeout,
                SendTimeout    = _config.SendTimeout,
                NoDelay        = false
            };
        }

        public void Close()
        {
#if DNX451
            _tcpClient.Close();
#else
            _tcpClient.Dispose();
#endif
            DisposeOfConnectionResources();
        }

        /// <summary>
        /// Ensures that the socket is connected.
        /// </summary>
        /// <returns>A task to wait for the connection to be made.</returns>
        public async Task ConnectAsync()
        {
            if (!_tcpClient.Connected)
            {
                DisposeOfConnectionResources();
                await _tcpClient.ConnectAsync(_config.Host, _config.Port).ConfigureAwait(false);
                _stream         = _tcpClient.GetStream();
                _responseReader = new ResponseReader(_stream);
                _commandWriter  = new CommandWriter(_stream);
            }
        }

        public void Dispose()
        {
            Close();
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
            await ConnectAsync().ConfigureAwait(false);
            await _commandWriter.WriteRedisCommandAsync(command, parameters).ConfigureAwait(false);
            await _stream.FlushAsync();
        }

        /// <summary>
        /// Disposes of resources associated with the connection.
        /// </summary>
        void DisposeOfConnectionResources()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream         = null;
                _responseReader = null;
                _commandWriter  = null;
            }
        }
    }
}
