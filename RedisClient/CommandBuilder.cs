using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munq.Redis
{
    public class CommandBuilder
    {
        private void AddObjectToCommand(StringBuilder sb, object obj)
        {
            var objType = obj.GetType();
            if (objType == typeof(Boolean))
            {
                obj = (bool)obj ? "1" : "0";
            }
            AddStringToCommand(sb, obj.ToString());
        }
        private void AddStringToCommand(StringBuilder sb, string value)
        {
            sb.Append('$');
            sb.Append(value.Length);
            sb.AppendLine();
            sb.AppendLine(value);
        }

        public byte[] CreateCommandData(string command, IEnumerable<object> parameters)
        {
            var sb = new StringBuilder();

            var sizeOfCommandArray = 1 + (parameters != null ? parameters.Count() : 0);
            sb.Append("*");
            sb.Append(sizeOfCommandArray);
            sb.AppendLine();
            AddStringToCommand(sb, command);

            if (sizeOfCommandArray > 1)
            {
                foreach (object obj in parameters)
                {
                    AddObjectToCommand(sb, obj);
                }
            }

            var commandString = sb.ToString();
            return Encoding.UTF8.GetBytes(commandString);
        }
    }
}
