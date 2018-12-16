using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Collections.Generic;
using System.Text;

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

                //ToDo: fix recursive func call
                si.Properties = InterfaceInfoBuilder.BuildPropInfoCol(t);


                Structs.Add(si.Name, si);
            }
        }
    }
    public class StructInfo
    {
        public string Name { get; set; }

        public PropertyInfoCollection Properties { get; set; } = new PropertyInfoCollection();

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"struct {Name} {{");

            //ToDo: continue implement struct collector
            sb.Append(Utils.Indent(Properties.ToString()));

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
