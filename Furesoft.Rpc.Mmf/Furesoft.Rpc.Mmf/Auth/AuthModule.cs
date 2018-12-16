using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Furesoft.Rpc.Mmf.Auth
{
    public static class AuthModule
    {
        public static bool IsAuthenticatet = false;
        public static List<string> Claims = new List<string>();

        public static Guid AppID;
        internal static string Reason = "RCA";
        internal static Guid Stamp = Guid.Parse("FDDBD5D4-1661-4D84-8DBF-070319EF83F1");

        public static void Enable(RpcBootstrapper bstrp)
        {
            bstrp.AfterRequest += Bstrp_AfterRequest;
            bstrp.BeforeRequest += Bstrp_BeforeRequest;
        }

        private static RpcMessage Bstrp_BeforeRequest(RpcMessage arg1, Type arg2, bool clientMode)
        {
            if (clientMode)
            {
                arg1.AddHeader("Authentication: " + Token.GenerateToken(Reason, AppID, Stamp));
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
                    var token = Token.ValidateToken(Reason, t, Stamp, AppID);

                    if (token != null)
                    {
                        if (token.Validated)
                        {
                            if (Claims.Contains(attr.Claim))
                            {
                                return arg1.ReturnValue;
                            }

                            return ThrowException(method);
                        }
                        else
                        {
                            return ThrowException(method, "Token Validation Error: " + Enum.GetName(typeof(Token.TokenValidationStatus), token.Errors.First()));
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

        private static object ThrowException(MethodInfo method, string msg = "")
        {
            var expt = new MethodAccessException(msg);
            Singleton<ExceptionStack>.Instance.Push(expt);

            return Activator.CreateInstance(method.ReturnType);
        }
    }
}