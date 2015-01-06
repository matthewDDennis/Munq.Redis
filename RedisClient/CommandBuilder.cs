using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Munq.Redis
{
    /// <summary>
    /// The command builder is a static class which is used to build Redis Server commands from and command
    /// name and a collection of parameters.
    /// </summary>
    /// <remarks>This class is safe for concurrent, multi-thread access.</remarks>
    public class CommandBuilder : IDisposable
    {
        MemoryStream memoryStream = new MemoryStream();

        /// <summary>
        /// Builds an array of bytes to send to the Redis Server for the command and it's parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The paramaters for the command.</param>
        /// <returns>The bytes to send to the Redis Server.</returns>
        public byte[] CreateCommandData(string command, IEnumerable<object> parameters)
        {
            if (string.IsNullOrWhiteSpace(command))
                throw new ArgumentNullException("command");

            var sizeOfCommandArray = 1 + (parameters != null ? parameters.Count() : 0);
            Append("*{0}\r\n", sizeOfCommandArray);
            AddStringToCommand(command);

            if (sizeOfCommandArray > 1)
            {
                foreach (object obj in parameters)
                    AddObjectToCommand(obj);
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Adds an object to the command data.
        /// </summary>
        /// <param name="value">The object to add.</param>
        void AddObjectToCommand(object value)
        {
            var objType = value.GetType();
            if (objType == typeof(byte[]))
                AddBytesToCommand(value as byte[]);
            else
            {
                if (objType == typeof(bool))
                {
                    value = (bool)value ? "1" : "0";
                }
                AddStringToCommand(value.ToString());
            }
        }

        /// <summary>
        /// Adds string to the command data.
        /// </summary>
        /// <param name="value">The string to add.</param>
        void AddStringToCommand(string value)
        {
            if (value != null)
                Append("${0}\r\n{1}\r\n", value.Length, value);
            else
                Append("$-1\r\n");
        }

        void AddBytesToCommand(byte[] bytes)
        {
            if (bytes != null)
            {
                Append("${0}\r\n", bytes.Length);
                memoryStream.Write(bytes, 0, bytes.Length);
                Append("\r\n");
            }
            else
                Append("$-1\r\n");
        }

        void Append(string format, params object[] parameters)
        {
            string formattedString = string.Format(format, parameters);
            byte[] data            = Encoding.UTF8.GetBytes(formattedString);
            memoryStream.Write(data, 0, data.Length);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    memoryStream.Dispose();        
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources. 
        // ~CommandBuilder() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
