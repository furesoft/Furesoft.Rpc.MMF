using System;
using System.Collections.Generic;
using System.Reflection;

namespace Furesoft.Rpc.Mmf.Auth
{
    public static class AuthModule
    {
        public static bool IsAuthenticatet = false;
        public static List<string> Claims = new List<string>();

        public static void Enable(RpcBootstrapper bstrp)
        {
            bstrp.AfterRequest += Bstrp_BeforeRequest;
        }

        private static object Bstrp_BeforeRequest(Messages.RpcMethodAwnser arg1, System.Type arg2)
        {
            var method = arg2.GetMethod(arg1.Name);
            var attr = method?.GetCustomAttribute<AuthAttribute>();

            if(attr == null)
            {
                return arg1.ReturnValue;
            }
            else
            {
                if(Claims.Contains(attr.Claim))
                {
                    return arg1.ReturnValue;
                }
                else
                {
                    var expt = new MethodAccessException();
                    Singleton<ExceptionStack>.Instance.Push(expt);

                    return Activator.CreateInstance(method.ReturnType);
                }
            }
        }
    }
}