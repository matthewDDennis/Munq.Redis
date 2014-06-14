using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisClient : IDisposable
    {
        private readonly RedisClientConfig _config;
        private readonly TcpClient         _tcpClient;
        private readonly CommandBuilder    _commandBuilder;
        private          ResponseReader    _reader;
        private          NetworkStream     _stream;

        public RedisClient() : this(new RedisClientConfig())
        {
        }

        public RedisClient(RedisClientConfig config)
        {
            _config         = config;
            _commandBuilder = new CommandBuilder();
            _tcpClient      = new TcpClient() 
            { 
                ReceiveTimeout = _config.ReceiveTimeout,
                SendTimeout    = _config.SendTimeout,
                NoDelay        = true
            };
        }

        private void DisposeOfConnectionResources()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }

            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }

        public void Close()
        {
            _tcpClient.Close();
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
                _stream = _tcpClient.GetStream();
                _reader = new ResponseReader(_stream);
            }
        }
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Gets the response reader.
        /// </summary>
        /// <returns>The response reade.</returns>
        public async Task<object> ReadResponseAsync()
        {
            try
            {
                await ConnectAsync();
                return await _reader.ReadAsync();
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
        public async Task SendAsync(string command, params object[] parameters)
        {
            await SendAsync(command, (IEnumerable<object>)parameters);
        }

        /// <summary>
        /// Sends a command and returns the response string.
        /// </summary>
        /// <param name="command">The Command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The response.</returns>
        public async Task SendAsync(string command, IEnumerable<object> parameters)
        {
            await ConnectAsync().ConfigureAwait(false);
            var commandData = _commandBuilder.CreateCommandData(command, parameters);
            await _stream.WriteAsync(commandData, 0, commandData.Length).ConfigureAwait(false);
        }
    }
}
