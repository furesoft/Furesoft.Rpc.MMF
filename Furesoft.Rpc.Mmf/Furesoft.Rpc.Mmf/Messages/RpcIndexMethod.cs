using System;

namespace Furesoft.Rpc.Mmf.Messages
{
    [Serializable]
    public class RpcIndexMethod : RpcMethod
    {
        public object[] Indizes { get; set; }
        public object Value { get; set; }
    }
}