using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [Serializable]
    public class StructInfo
    {
        public string Name { get; set; }

        public PropertyInfoCollection Properties { get; set; } = new PropertyInfoCollection();

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"struct {Name} {{");

            sb.Append(Utils.Indent(Properties.ToString()));

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}