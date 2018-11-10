using Furesoft.Rpc.Mmf.Messages;
using Furesoft.Rpc.Mmf.Proxy;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Furesoft.Rpc.Mmf
{
    public class RpcClient : IDisposable
    {
        //ToDo: make it generic to replace the method like to tcp
        private IRpcChannel sender;
        private IRpcChannel events;

        public RpcClient(string name, IRpcChannel channel, IRpcChannel eventChannel)
        {
            sender = channel;
            sender.MessageRecieved += Sender_DataReceived;
            
            events = eventChannel;
            events.MessageRecieved += Events_DataReceived;
         }

        private void Events_DataReceived(byte[] data)
        {
            var response = RpcServices.Deserialize(data);

            if (response is RpcEventCallMessage ev)
            {
                InvokeHandlers(ev);
            }
        }

        public void Start()
        {
            sender.Start();
            events.Start();
        }

        [DebuggerStepThrough]
        private void Sender_DataReceived(byte[] data)
        {
            var response = RpcServices.Deserialize(data);

            if (response is RpcMethodAwnser awnser)
            {
                ReturnValue = awnser.ReturnValue;
            }
            else if (response is RpcExceptionMessage ex)
            {
                throw new RpcException(ex.Interface, ex.Name, new Exception(ex.Message));
            }

            mre.Set();
        }

        internal void InvokeHandlers(RpcEventCallMessage ev)
        {
            var e = RpcEventRepository.Get(ev.Name);
            e.Invoke(ev.Args[0], (EventArgs)ev.Args[1]);
        }

        object ReturnValue;
        ManualResetEvent mre = new ManualResetEvent(false);

        public object CallMethod<Interface>(string methodname, params object[] args)
            where Interface : class
        {
            mre.Reset();

            var m = new RpcMethod
            {
                Interface = typeof(Interface).Name,
                Name = methodname,
                Args = args.ToList()
            };

            sender.SendMessage(RpcServices.Serialize(m));

            mre.WaitOne();

            return ReturnValue;
        }

        public T CallMethod<Interface, T>(string methodname, params object[] args)
            where Interface : class
        {
            return (T)CallMethod<Interface>(methodname, args);
        }

        public Task<object> CallMethodAsync<Interface>(string methodname, params object[] args)
            where Interface : class
        {
            return Task.Run(() =>
            {
                return CallMethod<Interface>(methodname, args);
            });
        }
        public Task<T> CallMethodAsync<Interface, T>(string methodname, params object[] args)
            where Interface : class
        {
            return Task.Run(() =>
            {
                return CallMethod<Interface, T>(methodname, args);
            });
        }


        public void SetProperty<Interface>(string propname, object value)
            where Interface : class
        {
            CallMethod<Interface>($"set_{propname}", value);
        }

        public void SetIndex<Interface>(object[] indizes, object value)
        {
            mre.Reset();

            var m = new RpcIndexMethod
            {
                Name = "set_Index",
                Interface = typeof(Interface).Name,
                Indizes = indizes,
                Value = value
            };

            sender.SendMessage(RpcServices.Serialize(m));

            mre.WaitOne();
        }
        public object GetIndex<Interface>(object[] indizes)
        {
            mre.Reset();

            var m = new RpcIndexMethod
            {
                Interface = typeof(Interface).Name,
                Name = "get_Index",
                Indizes = indizes
            };

            sender.SendMessage(RpcServices.Serialize(m));

            mre.WaitOne();

            return ReturnValue;
        }

        public object GetProperty<Interface>(string propertyname)
            where Interface : class
        {
            return CallMethod<Interface>($"get_{propertyname}");
        }

        public dynamic BindDynamic<Interface>()
            where Interface : class
        {
            return new InterfaceProxy<Interface>(this);
        }

        public Interface Bind<Interface>()
            where Interface : class
        {
            return Impromptu.ActLike<Interface>(new InterfaceProxy<Interface>(this));
        }
        
        public void Dispose()
        {
            sender.Dispose();
            events.Dispose();
        }
    }
}