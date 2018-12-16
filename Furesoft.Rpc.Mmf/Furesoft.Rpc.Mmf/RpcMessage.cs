using System;

namespace Furesoft.Rpc.Mmf
{
    [Serializable]
    public abstract class RpcMessage
    {
        public HeaderCollection Headers { get; set; } = new HeaderCollection();

        public string Interface { get; set; }
        public string Name { get; set; }
    }
}