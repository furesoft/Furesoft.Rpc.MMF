using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi.Collections
{
    [Serializable]
    public class PropertyInfoCollection : Collection<PropertyInfo>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var fi in this)
            {
                sb.AppendLine();
                sb.AppendLine(fi.ToString());
            }

            return sb.ToString();
        }
    }
}