using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf.Messages
{
    [Serializable]
    public class RpcMethod : RpcMessage
    {
        public List<object> Args { get; set; } = new List<object>();
    }
}