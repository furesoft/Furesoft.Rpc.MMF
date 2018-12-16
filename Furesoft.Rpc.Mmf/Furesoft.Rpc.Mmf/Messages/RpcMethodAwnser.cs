using System;

namespace Furesoft.Rpc.Mmf.Messages
{
    [Serializable]
    public class RpcMethodAwnser : RpcMessage
    {
        public object ReturnValue { get; set; }
    }
}