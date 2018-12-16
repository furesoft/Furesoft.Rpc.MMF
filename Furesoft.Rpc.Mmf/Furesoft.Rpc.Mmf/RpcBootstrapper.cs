using Furesoft.Rpc.Mmf.Messages;
using System;

namespace Furesoft.Rpc.Mmf
{
    public class RpcBootstrapper
    {
        public event Func<RpcMessage, Type, bool, RpcMessage> BeforeRequest;
        public event Func<RpcMethodAwnser, Type,bool, object> AfterRequest;

        internal RpcMessage OnBeforeRequest(RpcMessage msg, Type type, bool clientMode)
        {
            return BeforeRequest?.Invoke(msg, type, clientMode);
        }

        internal object OnAfterRequest(RpcMethodAwnser msg, Type type, bool clientMode)
        {
            return AfterRequest?.Invoke(msg, type, clientMode);
        }

        public virtual void HandleRequest(RpcMessage msg, RpcServer server) { }
        public virtual void HandleRequest(RpcMessage msg, RpcClient client) { }

        public virtual void Boot() { }
    }
}