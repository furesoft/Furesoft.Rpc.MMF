using Furesoft.Rpc.Mmf.Communicator;
using Furesoft.Rpc.Mmf.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Furesoft.Rpc.Mmf
{
    public class RpcServer : IDisposable
    {
        private MemoryMappedFileCommunicator listener;
        private Dictionary<string, object> _binds = new Dictionary<string, object>();

        public RpcServer(string name)
        {
            listener = new MemoryMappedFileCommunicator(name, 50000);

            listener.WritePosition = 0;
            listener.ReadPosition = 2500;

            listener.DataReceived += Listener_DataReceived;
        }

        public void Bind<Interface>(Interface obj)
            where Interface : class
        {
            if (!_binds.ContainsKey(typeof(Interface).Name))
            {
                _binds.Add(typeof(Interface).Name, obj);
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

        public void CallEvent(string name, object sender, EventArgs e)
        {
            var msg = new RpcEventCallMessage
            {
                Name = name,
                Args = new List<object> { sender, e }
            };

            listener.Write(RpcServices.Serialize(msg));
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

            r = p.Invoke(_binds[method.Interface], args);

            if (r is Exception ex)
            {
                listener.Write(
                    RpcServices.Serialize(
                        new RpcExceptionMessage(
                            method.Interface,
                            method.Name,
                            ex.Message
                    ))
               );
            }

            return r;
        }

        private void Listener_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
        {
            var method = RpcServices.Deserialize(e.Data);

            object r = null;

            if (_binds.ContainsKey(method.Interface))
            {
                var type = _binds[method.Interface].GetType();

                if (method is RpcIndexMethod ri)
                {
                    if (ri.Name == "get_Index")
                    {
                        var p = GetIndexProperties(_binds[method.Interface]).First();

                        r = InvokeMethod(p, ri, ri.Indizes);
                    }
                    else
                    {
                        var p = SetIndexProperties(_binds[method.Interface]).First();
                        var args = new List<object>();
                        args.AddRange(ri.Indizes);
                        args.Add(ri.Value);

                        InvokeMethod(p, ri, args.ToArray());
                    }
                }
                else if(method is RpcMethod rm)
                {
                    var name = rm.Name.Replace("get_", "");

                    if(name.StartsWith("On"))
                    {
                        r = RpcEventRepository.Get(name);
                        return;
                    }

                    var m = type.GetMethod(method.Name);

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
                    Interface = method.Interface,
                    Name = method.Name,
                    ReturnValue = r
                };

                listener.Write(RpcServices.Serialize(returner));
            }
            else
            {
                throw new Exception($"Interface '{method.Interface}' is not bound!");
            }
        }
    }
}