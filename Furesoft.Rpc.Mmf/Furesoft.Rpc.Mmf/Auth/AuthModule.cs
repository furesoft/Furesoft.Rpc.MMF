using System;
using System.Collections.Generic;
using System.Reflection;

namespace Furesoft.Rpc.Mmf.Auth
{
    public static class AuthModule
    {
        public static bool IsAuthenticatet = false;
        public static List<string> Claims = new List<string>();

        public static TimeSpan ExpireToken;

        public static void Enable(RpcBootstrapper bstrp, TimeSpan expireToken)
        {
            bstrp.AfterRequest += Bstrp_AfterRequest;
            bstrp.BeforeRequest += Bstrp_BeforeRequest;

            ExpireToken = expireToken;
        }

        private static RpcMessage Bstrp_BeforeRequest(RpcMessage arg1, Type arg2, bool clientMode)
        {
            if (clientMode)
            {
                arg1.AddHeader("Authentication: " + Token.Create(Guid.NewGuid()));
            }

            return arg1;
        }

        private static object Bstrp_AfterRequest(Messages.RpcMethodAwnser arg1, System.Type arg2, bool clientMode)
        {
            var method = arg2.GetMethod(arg1.Name);
            var attr = method?.GetCustomAttribute<AuthAttribute>();

            if (!clientMode)
            {
                if (attr == null)
                {
                    return arg1.ReturnValue;
                }
                else
                {
                    var t = arg1.GetHeader("Authentication");
                    var token = Token.Parse(t);

                    if (token != null)
                    {
                        if (token.Validate(ExpireToken))
                        {
                            if (Claims.Contains(attr.Claim))
                            {
                                return arg1.ReturnValue;
                            }

                            return ThrowException(method);
                        }
                        else
                        {
                            return ThrowException(method);
                        }
                    }
                    else
                    {
                        return ThrowException(method);
                    }
                }
            }
            else
            {
                return arg1.ReturnValue;
            }
        }

        private static object ThrowException(MethodInfo method)
        {
            var expt = new MethodAccessException();
            Singleton<ExceptionStack>.Instance.Push(expt);

            return Activator.CreateInstance(method.ReturnType);
        }
    }
}