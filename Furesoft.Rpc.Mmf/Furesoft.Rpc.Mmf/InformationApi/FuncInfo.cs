using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [Serializable]
    public class FuncInfo : InterfaceInfoPart
    {
        public ArgumentCollection Arguments { get; set; } = new ArgumentCollection();
        public string ReturnType { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(base.ToString());

            sb.AppendLine($"func {Name}({Arguments.ToString()}) -> {ReturnType};\r\n");
            sb.AppendLine("");

            return sb.ToString().InsertLast(Environment.NewLine);
        }
    }
}