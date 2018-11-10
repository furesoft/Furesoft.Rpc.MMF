using Furesoft.Rpc.Mmf.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Furesoft.Rpc.Mmf
{
    public class RpcServer : IDisposable
    {
        private IRpcChannel listener;
        private Dictionary<string, object> _binds = new Dictionary<string, object>();

        public RpcServer(string name, IRpcChannel channel)
        {
            listener = channel;

            listener.MessageRecieved += Listener_DataReceived;
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
            listener.Start();
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
                listener.SendMessage(
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

        private void Listener_DataReceived(byte[] data)
        {
            var method = RpcServices.Deserialize(data);

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
                else if (method is RpcMethod rm)
                {
                    var name = rm.Name.Replace("get_", "");

                    if (name.StartsWith("On"))
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

                listener.SendMessage(RpcServices.Serialize(returner));
            }
            else
            {
                throw new Exception($"Interface '{method.Interface}' is not bound!");
            }
        }
    }
}