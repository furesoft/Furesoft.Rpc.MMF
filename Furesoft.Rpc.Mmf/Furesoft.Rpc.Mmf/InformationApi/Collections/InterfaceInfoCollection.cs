using System.Collections.ObjectModel;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    public class InterfaceInfoCollection : Collection<InterfaceInfo>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var ii in this)
            {
                sb.AppendLine(ii.ToString());
            }

            sb.AppendLine();

            foreach (var s in StructCollector.Structs)
            {
                sb.AppendLine(s.ToString());
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}