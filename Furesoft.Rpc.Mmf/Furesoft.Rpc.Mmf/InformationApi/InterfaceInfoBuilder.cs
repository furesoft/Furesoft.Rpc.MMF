using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Xaml;
using Furesoft.Rpc.Mmf.InformationApi;
using Furesoft.Rpc.Mmf.InformationApi.Attributes;
using Furesoft.Rpc.Mmf.InformationApi.Collections;

namespace Furesoft.Rpc.Mmf
{
    internal class InterfaceInfoBuilder
    {
        internal static InterfaceInfo StartBuild(Type iType)
        {
            var ret = new InterfaceInfo();
            ret.Name = iType.Name;
            ret.Description = GetDescription(iType);
            ret.Functions = BuildFuncInfoColl(iType);
            ret.ThrowsExceptionInfo = BuildExceptionInfo(iType);
            ret.Properties = BuildPropInfoCol(iType);
           
            
            //ToDo: implement collect interface info

            return ret;
        }

        internal static FuncInfoCollection BuildFuncInfoColl(Type iType)
        {
            var funcs = iType.GetMethods();
            var ret = new FuncInfoCollection();

            var fs = funcs.Where(_ => !_.Name.StartsWith("get_")).Where(_ => !_.Name.StartsWith("set_"));

            foreach (var f in fs)
            {
                ret.Add(BuildFuncInfo(f));
            }

            return ret;
        }

        internal static FuncInfo BuildFuncInfo(MethodInfo f)
        {
            var fi = new FuncInfo();

            fi.Name = f.Name;
            fi.Description = GetDescription(f);
            fi.ReturnType = f.ReturnType.Name;
            fi.NeedsAuth = f.GetCustomAttribute<Auth.AuthAttribute>() != null;

            if(f.ReturnType.IsValueType && !StructCollector.Structs.ContainsKey(f.ReturnType.Name))
            {
                //StructCollector.CollectType(f.ReturnType);
            }

            fi.ThrowsExceptionInfo = BuildExceptionInfo(f);
            fi.Arguments = BuildArgInfoCol(f.GetParameters());
            //ToDo: continue Funcinfo build!

            return fi;
        }
        internal static InformationApi.PropertyInfo BuildPropInfo(System.Reflection.PropertyInfo f)
        {
            var fi = new InformationApi.PropertyInfo();

            fi.Name = f.Name;
            fi.Description = GetDescription(f);
            fi.Type = f.PropertyType.Name;
            fi.CanGet = f.CanRead;
            fi.CanSet = f.CanWrite;

            foreach (var pi in f.GetIndexParameters())
            {
                fi.Indizes.Add(BuildArgInfo(pi));
            }

            if (f.PropertyType.IsValueType && !StructCollector.Structs.ContainsKey(f.PropertyType.Name))
            {
                StructCollector.CollectType(f.PropertyType);
            }

            fi.ThrowsExceptionInfo = BuildExceptionInfo(f);
            //ToDo: continue Funcinfo build!

            return fi;
        }
        internal static InformationApi.Collections.PropertyInfoCollection BuildPropInfoCol(Type t)
        {
            var props = t.GetProperties();
            var ret = new PropertyInfoCollection();

            foreach (var p in props)
            {
                ret.Add(BuildPropInfo(p));
            }

            return ret;
        }

        private static ExceptionInfo BuildExceptionInfo(MemberInfo f)
        {
            var att = f.GetCustomAttributes<ThrowsExceptionAttribute>();

            return BuildExceptionInfo(att);
        }
        private static ExceptionInfo BuildExceptionInfo(Type f)
        {
            var att = f.GetCustomAttributes<ThrowsExceptionAttribute>();

            return BuildExceptionInfo(att);
        }
        private static ExceptionInfo BuildExceptionInfo(ParameterInfo f)
        {
            var att = f.GetCustomAttributes<ThrowsExceptionAttribute>();

            return BuildExceptionInfo(att);
        }
        private static ExceptionInfo BuildExceptionInfo(IEnumerable<ThrowsExceptionAttribute> att)
        {
            if (att.Any())
            {
                var ei = new ExceptionInfo();

                foreach (var e in att)
                {
                    ei.Add(e.Exception);
                }

                return ei;
            }

            return new ExceptionInfo();
        }

        internal static ArgumentCollection BuildArgInfoCol(ParameterInfo[] info)
        {
            var ret = new ArgumentCollection();

            foreach (var arg in info)
            {
                ret.Add(BuildArgInfo(arg));
            }

            return ret;
        }

        internal static ArgumentInfo BuildArgInfo(ParameterInfo arg)
        {
            var ai = new ArgumentInfo();
            ai.Name = arg.Name;
            ai.Description = GetDescription(arg);
            ai.IsOptional = arg.IsOptional;
            ai.Type = arg.ParameterType.Name;
            ai.ThrowsExceptionInfo = BuildExceptionInfo(arg);

            //ToDo: continue argumentinfo build!

            return ai;
        }

        private static string GetDescription(MemberInfo iType)
        {
            try
            {
                var att = iType.GetCustomAttribute<DescriptionAttribute>();

                if (att != null)
                {
                    return att.Description;
                }
            }
            catch { }

            return string.Empty;
        }

        private static string GetDescription(ParameterInfo iType)
        {
            var att = iType.GetCustomAttribute<DescriptionAttribute>();

            if (att != null)
            {
                return att.Description;
            }

            return string.Empty;
        }
    }
}