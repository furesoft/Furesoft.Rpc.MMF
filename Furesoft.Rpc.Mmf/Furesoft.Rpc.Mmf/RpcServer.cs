using Furesoft.Rpc.Mmf.Communicator;
using Furesoft.Rpc.Mmf.InformationApi;
using Furesoft.Rpc.Mmf.Messages;
using Furesoft.Rpc.Mmf.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Furesoft.Rpc.Mmf
{
    public class RpcServer : IDisposable
    {
        private MemoryMappedFileCommunicator listener;
        internal Dictionary<string, object> _binds = new Dictionary<string, object>();

        internal Dictionary<string, Type> _iTypes = new Dictionary<string, Type>();

        internal Type GetInterfaceType(string name)
        {
            return _iTypes[name];
        }

        public RpcBootstrapper Bootstrapper;
        private readonly RpcSerializer Serializer;

        public RpcServer(string name, RpcBootstrapper bootstrp = null, RpcSerializer serializer = null)
        {
            listener = new MemoryMappedFileCommunicator(name, 50000);

            listener.WritePosition = 0;
            listener.ReadPosition = 2500;
            listener.DataReceived += Listener_DataReceived;

            Bootstrapper = bootstrp;
            this.Serializer = serializer;
            if (Serializer == null) Serializer = new XamlSerializer();

            Bind<IInterfaceInfo>(new InterfaceInfoImpl(this));

            Bootstrapper?.Boot();
        }

        public void Bind<Interface>(Interface obj)
            where Interface : class
        {
            Type type = typeof(Interface);

            if (!_binds.ContainsKey(type.Name))
            {
                _binds.Add(type.Name, obj);
                _iTypes.Add(type.Name, type);
            }
        }

        public void Dispose()
        {
            listener.Dispose();
        }

        public void Start()
        {
            listener.StartReader();
        }

        public IList<MethodInfo> GetIndexProperties(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var type = obj.GetType();
            IList<MethodInfo> results = new List<MethodInfo>();

            try
            {
                var props = type.GetProperties(System.Reflection.BindingFlags.Default |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);

                if (props != null)
                {
                    foreach (var prop in props)
                    {
                        var indexParameters = prop.GetIndexParameters();
                        if (indexParameters == null || indexParameters.Length == 0)
                        {
                            continue;
                        }
                        var getMethod = prop.GetGetMethod();
                        if (getMethod == null)
                        {
                            continue;
                        }
                        results.Add(getMethod);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return results;
        }
        public IList<MethodInfo> SetIndexProperties(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var type = obj.GetType();
            IList<MethodInfo> results = new List<MethodInfo>();

            try
            {
                var props = type.GetProperties(System.Reflection.BindingFlags.Default |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);

                if (props != null)
                {
                    foreach (var prop in props)
                    {
                        var indexParameters = prop.GetIndexParameters();
                        if (indexParameters == null || indexParameters.Length == 0)
                        {
                            continue;
                        }
                        var getMethod = prop.GetSetMethod();
                        if (getMethod == null)
                        {
                            continue;
                        }
                        results.Add(getMethod);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return results;
        }

        object InvokeMethod(MethodInfo p, RpcMethod method, params object[] args)
        {
            object r = null;

            try
            {
                r = p.Invoke(_binds[method.Interface], args);
            }
            catch(Exception ex)
            {
                Singleton<ExceptionStack>.Instance.Push(ex);
            }

            return r;
        }

        private void Listener_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
        {
            var msg = Serializer.Deserialize(e.Data);

            Bootstrapper.HandleRequest(msg, this);

            object r = null;

            if (msg == null) return;

            if (_binds.ContainsKey(msg.Interface))
            {
                var type = _binds[msg.Interface].GetType();

                msg = Bootstrapper.OnBeforeRequest(msg, type, false);

                if (msg is RpcIndexMethod ri)
                {
                    if (ri.Name == "get_Index")
                    {
                        var p = GetIndexProperties(_binds[msg.Interface]).First();

                        r = InvokeMethod(p, ri, ri.Indizes);
                    }
                    else
                    {
                        var p = SetIndexProperties(_binds[msg.Interface]).First();
                        var args = new List<object>();
                        args.AddRange(ri.Indizes);
                        args.Add(ri.Value);

                        InvokeMethod(p, ri, args.ToArray());
                    }
                }
                else if(msg is RpcMethod rm)
                {
                    var name = rm.Name.Replace("get_", "");

                    if(name.StartsWith("On"))
                    {
                        r = RpcEventRepository.Get(name);
                        return;
                    }

                    var m = type.GetMethod(msg.Name);

                    if (m?.ReturnType == typeof(void))
                    {
                        r = null;

                        InvokeMethod(m, rm, rm.Args.ToArray());
                    }
                    else
                    {
                        r = InvokeMethod(m, rm, rm.Args.ToArray());
                    }
                }

                var returner = new RpcMethodAwnser()
                {
                    Interface = msg.Interface,
                    Name = msg.Name,
                    ReturnValue = r
                };
                returner.Headers = msg.Headers;
                //ToDo: fix headers

                var ret = Bootstrapper.OnAfterRequest(returner, _iTypes[msg.Interface], false);

                var exSt = Singleton<ExceptionStack>.Instance;
                if(exSt.Any())
                {
                    var errMsg = new RpcExceptionMessage(msg.Interface, msg.Name, exSt.Pop().ToString());
                    listener.Write(Serializer.Serialize(errMsg));

                    return;
                }

                returner.ReturnValue = ret;

                listener.Write(Serializer.Serialize(returner));
            }
            else
            {
                throw new Exception($"Interface '{msg.Interface}' is not bound!");
            }
        }
    }
}