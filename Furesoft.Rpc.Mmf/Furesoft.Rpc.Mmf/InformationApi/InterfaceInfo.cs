using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [Serializable]
    public class InterfaceInfo : InterfaceInfoPart
    {
        public EventInfoCollection Events { get; set; } = new EventInfoCollection();
        public FuncInfoCollection Functions { get; set; } = new FuncInfoCollection();
        public PropertyInfoCollection Properties { get; set; } = new PropertyInfoCollection();

        public override string ToString()
        {
            var meta = base.ToString();
            var sb = new StringBuilder();
            
            sb.Append(meta);
            sb.AppendLine($"interface {Name} {{");

            sb.Append(Utils.Indent(Functions.ToString()));
            sb.Append(Utils.Indent(Properties.ToString()));

            sb.AppendLine("}");

            sb.AppendLine();

            foreach (var s in StructCollector.Structs)
            {
                sb.Append(s.Value.ToString());
            }

            return sb.ToString();

        }
    }
}