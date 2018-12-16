using Furesoft.Rpc.Mmf.Communicator;
using Furesoft.Rpc.Mmf.InformationApi;
using Furesoft.Rpc.Mmf.Messages;
using Furesoft.Rpc.Mmf.Proxy;
using Furesoft.Rpc.Mmf.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Furesoft.Rpc.Mmf
{
    public class RpcClient : IDisposable, IInterfaceInfo
    {
        //ToDo: make it generic to replace the method like to tcp
        private MemoryMappedFileCommunicator sender;
        private MemoryMappedFileCommunicator events;

        public RpcBootstrapper Bootstrapper;


        public RpcClient(string name, RpcBootstrapper bootstrp = null, RpcSerializer serializer = null)
        {
            sender = new MemoryMappedFileCommunicator(name, 50000); ;

            sender.ReadPosition = 0;
            sender.WritePosition = 2500;
            sender.DataReceived += Sender_DataReceived;

            events = new MemoryMappedFileCommunicator(name + ".events", 5000);
            events.ReadPosition = 0;
            events.WritePosition = 2500;
            events.DataReceived += Events_DataReceived;

            Bootstrapper = bootstrp;
            Serializer = serializer;

            if (Serializer == null) Serializer = new XamlSerializer();

            Bootstrapper?.Boot();
        }

        private void Events_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
        {
            var response = Serializer.Deserialize(e.Data);

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
            var response = Serializer.Deserialize(e.Data);

            if (response is RpcMethodAwnser awnser)
            {
                ReturnValue = awnser.ReturnValue;
                var ret = Bootstrapper?.OnAfterRequest(awnser, Type.GetType(response.Interface));
                if (ret != null) ReturnValue = ret;
            }
            else if (response is RpcExceptionMessage ex)
            {
                Singleton<ExceptionStack>.Instance.Push(new RpcException(ex.Interface, ex.Name, new Exception(ex.Message)));
            }

            Bootstrapper?.HandleRequest(response, this);

            mre.Set();
        }

        internal void InvokeHandlers(RpcEventCallMessage ev)
        {
            var e = RpcEventRepository.Get(ev.Name);
            e.Invoke(ev.Args[0], (EventArgs)ev.Args[1]);
        }

        object ReturnValue;
        ManualResetEvent mre = new ManualResetEvent(false);

        public RpcSerializer Serializer { get; }

        [DebuggerStepThrough]
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

            Bootstrapper?.OnBeforeRequest(m, typeof(Interface));
            sender.Write(Serializer.Serialize(m));

            mre.WaitOne();

            if(Singleton<ExceptionStack>.Instance.Any())
            {
                throw Singleton<ExceptionStack>.Instance.Pop();
            }

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

            sender.Write(Serializer.Serialize(m));

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

            sender.Write(Serializer.Serialize(m));

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

        public InterfaceInfo GetInfo(string name)
        {
            var api = Bind<IInterfaceInfo>();

            return api.GetInfo(name);
        }

        public InterfaceInfo GetInfo<T>()
        {
            var api = Bind<IInterfaceInfo>();

            Thread.Sleep(10);

            return api.GetInfo(typeof(T).Name);
        }
    }
}