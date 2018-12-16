using Furesoft.Rpc.Mmf.Messages;
using System;

namespace Furesoft.Rpc.Mmf
{
    public class RpcBootstrapper
    {
        public event Action<RpcMessage, Type> BeforeRequest;
        public event Func<RpcMethodAwnser, Type, object> AfterRequest;

        internal void OnBeforeRequest(RpcMessage msg, Type type)
        {
            BeforeRequest?.Invoke(msg, type);
        }

        internal object OnAfterRequest(RpcMethodAwnser msg, Type type)
        {
            return AfterRequest?.Invoke(msg, type);
        }

        public virtual void HandleRequest(RpcMessage msg, RpcServer server) { }
        public virtual void HandleRequest(RpcMessage msg, RpcClient client) { }

        public virtual void Boot() { }
    }
}