using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class RedisClient : IDisposable
    {
        private const string DefaultHost = "localhost";
        private const int    DefaultPort = 6379;

        private readonly TcpClient _client;
        private readonly string    _host;
        private readonly int       _port;
        private NetworkStream      _stream;
        private CommandBuilder _commandBuilder;
        private ResponseReader     _reader;

        public RedisClient(string host = DefaultHost, int port = DefaultPort)
        {
            _host                  = host;
            _port                  = port;
            _client = new TcpClient() 
            { 
                ReceiveTimeout = 2000,
                SendTimeout    = 2000,
                NoDelay        = true 
            };
            _commandBuilder = new CommandBuilder();
       }

        /// <summary>
        /// Sends a command and returns the response string.
        /// </summary>
        /// <param name="command">The Command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The response.</returns>
        public async Task SendCommand(string command, params object[] parameters)
        {
            await ConnectAsync();
            byte[] commandData = _commandBuilder.CreateCommandData(command, parameters);
            await _stream.WriteAsync(commandData, 0, commandData.Length).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the response reader.
        /// </summary>
        /// <returns>The response reade.</returns>
        public async Task<object> ReadResponse()
        {
            //StringBuilder sb        = new StringBuilder();
            //byte[]        buffer    = new byte[_client.ReceiveBufferSize];
            //while (_stream.DataAvailable)
            //{
            //    int bytesRead = await _stream.ReadAsync(buffer, 0, _client.ReceiveBufferSize)
            //                                           .ConfigureAwait(false);
            //    if (bytesRead == 0)
            //        break;
            //    string dataString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            //    sb.Append(dataString);
            //}
            await ConnectAsync();
            return await _reader.Read();
        }

        /// <summary>
        /// Ensures that the socket is connected.
        /// </summary>
        /// <returns>A task to wait for the connection to be made.</returns>
        private async Task ConnectAsync()
        {
            if (!_client.Connected)
            {
                if (_stream != null)
                    _stream.Dispose();

                if (_reader != null)
                    _reader.Dispose();

                await _client.ConnectAsync(_host, _port).ConfigureAwait(false);
                _stream = _client.GetStream();
                _reader = new ResponseReader(_stream);
            }
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }

            _client.Close();
        }
    }
}