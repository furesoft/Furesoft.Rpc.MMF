using System;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    internal class Utils
    {
        public static string Indent(string v)
        {
            var lines = v.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            foreach (var l in lines)
            {
                sb.AppendLine($"\t{l}");
            }

            return sb.ToString();

        }
    }
}