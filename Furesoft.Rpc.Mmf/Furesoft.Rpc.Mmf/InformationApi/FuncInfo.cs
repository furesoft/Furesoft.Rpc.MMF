using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Text;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://furesoft.app/rpc/2018/", "Furesoft.Rpc.Mmf.InformationApi")]
[assembly: XmlnsDefinition("http://furesoft.app/rpc/2018/", "Furesoft.Rpc.Mmf.InformationApi.Collections")]

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [ContentProperty("Arguments")]
    [Serializable]
    public class FuncInfo : InterfaceInfoPart
    {
        public ArgumentCollection Arguments { get; set; } = new ArgumentCollection();
        public string ReturnType { get; set; }
        public bool NeedsAuth { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine(base.ToString());

            if(NeedsAuth)
            {
                sb.AppendLine("//NeedAuth");
            }

            sb.AppendLine($"func {Name}({Arguments.ToString()}) -> {ReturnType};\r\n");
            sb.AppendLine("");

            return sb.ToString().InsertLast(Environment.NewLine);
        }
    }
}