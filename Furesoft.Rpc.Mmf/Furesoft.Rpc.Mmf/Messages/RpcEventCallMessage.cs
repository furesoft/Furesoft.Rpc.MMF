using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf.Messages
{
    [Serializable]
    public sealed class RpcEventCallMessage : RpcMessage
    {
        public List<object> Args { get; set; } = new List<object>();
    }
}