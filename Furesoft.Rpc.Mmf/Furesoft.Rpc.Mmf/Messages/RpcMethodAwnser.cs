using System;
using System.Linq;

namespace Furesoft.Rpc.Mmf.Messages
{
    [Serializable]
    public class RpcMethodAwnser : RpcMessage
    {
        public object ReturnValue { get; set; }

        public string GetHeader(string v)
        {
            return Headers.FirstOrDefault( _=> _.StartsWith(v))?.Substring(v.Length + 2);
        }
    }
}