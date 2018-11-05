using Furesoft.Rpc.Mmf.Communicator;
using Furesoft.Rpc.Mmf.Messages;
using Furesoft.Rpc.Mmf.Proxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Furesoft.Rpc.Mmf
{
    public class RpcClient : IDisposable
    {
        //ToDo: make it generic to replace the method like to tcp
        private MemoryMappedFileCommunicator sender;
        private MemoryMappedFileCommunicator events;

        public RpcClient(string name)
        {
            sender = new MemoryMappedFileCommunicator(name, 50000); ;

            sender.ReadPosition = 0;
            sender.WritePosition = 2500;
            sender.DataReceived += Sender_DataReceived;

            events = new MemoryMappedFileCommunicator(name + ".events", 5000);
            events.ReadPosition = 0;
            events.WritePosition = 2500;
            events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
        {
            var response = RpcServices.Deserialize(e.Data);

            if (response is RpcEventCallMessage ev)
            {
                InvokeHandlers(ev);
            }
        }

        public void Start()
        {
            sender.StartReader();
            events.StartReader();
        }

        [DebuggerStepThrough]
        private void Sender_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
        {
            var response = RpcServices.Deserialize(e.Data);

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

            sender.Write(RpcServices.Serialize(m));

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

            sender.Write(RpcServices.Serialize(m));

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

            sender.Write(RpcServices.Serialize(m));

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
        }
    }
}