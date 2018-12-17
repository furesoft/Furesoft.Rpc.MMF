using Furesoft.Rpc.Mmf.InformationApi.Collections;
using System;
using System.Linq;
using System.Text;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [Serializable]
    public abstract class InterfaceInfoPart
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ExceptionInfo ThrowsExceptionInfo { get; set; } = new ExceptionInfo();

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(Description))
            {
                sb.AppendLine($"//Description: {Description}");
            }

            if (ThrowsExceptionInfo.Any())
            {
                sb.AppendLine($"//Exceptions: {ThrowsExceptionInfo.ToString()}");
            }

            return sb.ToString();
        }
    }
}