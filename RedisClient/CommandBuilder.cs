using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Munq.Redis
{
    public class CommandBuilder
    {
        public byte[] CreateCommandData(string command, IEnumerable<object> parameters)
        {
            StringBuilder sb = new StringBuilder();

            int sizeOfCommandArray = 1 + (parameters != null ? parameters.Count() : 0);
            sb.Append("*");
            sb.Append(sizeOfCommandArray);
            sb.AppendLine();
            AddStringToCommand(sb, command);

            if (sizeOfCommandArray > 1)
            {
                foreach (object obj in parameters)
                    AddObjectToCommand(sb, obj);
            }

            string commandString = sb.ToString();
            return Encoding.UTF8.GetBytes(commandString);
        }

        private void AddStringToCommand(StringBuilder sb, string value)
        {
            sb.Append('$');
            sb.Append(value.Length);
            sb.AppendLine();
            sb.AppendLine(value);
        }

        private void AddObjectToCommand(StringBuilder sb, object obj)
        {
            var objType = obj.GetType();
            if (objType == typeof(Boolean))
                obj = (bool)obj ? "1" : "0";

            AddStringToCommand(sb, obj.ToString());
        }
    }
}
