using System;
using System.Collections.ObjectModel;

namespace Furesoft.Rpc.Mmf.InformationApi.Collections
{
    [Serializable]
    public class ExceptionInfo : Collection<string>
    {
        public override string ToString()
        {
            return string.Join(", ", this);
        }
    }
}