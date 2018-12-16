using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi.Collections
{
    [Serializable]
    public class ArgumentCollection : Collection<ArgumentInfo>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var ai in this)
            {
                sb.Append(ai.ToString() + ", ");
            }

            if(sb.ToString().EndsWith(", "))
            {
                return sb.ToString().RemoveLast(2);
            }

            return sb.ToString();
        }
    }
}