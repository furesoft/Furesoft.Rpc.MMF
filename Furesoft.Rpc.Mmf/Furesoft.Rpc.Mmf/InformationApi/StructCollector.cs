using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    internal static class StructCollector
    {
        public static Dictionary<string, StructInfo> Structs = new Dictionary<string, StructInfo>();

        public static void CollectType(Type t)
        {
            if (!Structs.ContainsKey(t.Name))
            {
                var si = new StructInfo();
                si.Name = t.Name;

                si.Properties = InterfaceInfoBuilder.BuildPropInfoCol(t);

                Structs.Add(si.Name, si);
            }
        }
    }
}