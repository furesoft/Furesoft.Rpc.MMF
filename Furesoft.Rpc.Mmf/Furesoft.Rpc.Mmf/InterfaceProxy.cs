using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace Furesoft.Rpc.Mmf
{
    public class InterfaceProxy<Interface> : DynamicObject
        where Interface : class
    {
        private RpcClient rpcClient;
        
        public InterfaceProxy(RpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if(binder.Name.EndsWith("Async"))
            {
                result = Task.Run(()=> rpcClient.CallMethod<Interface>(binder.Name, args));
                return true;
            }

            result = rpcClient.CallMethod<Interface>(binder.Name, args);

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Run(() =>
                rpcClient.SetProperty<Interface>(binder.Name, value) //.Wait();
            );

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Call(() => rpcClient.GetProperty<Interface>(binder.Name));

            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = Call(() => rpcClient.GetIndex<Interface>(indexes));

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            Run(() =>
                rpcClient.SetIndex<Interface>(indexes, value)
            );

            return true;
        }

        public void Run(Action act)
        {
            act();
        }
        public T Call<T>(Func<T> act)
        {
            return act();
        }
    }
}