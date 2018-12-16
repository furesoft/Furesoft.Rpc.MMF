using System;

namespace Furesoft.Rpc.Mmf.InformationApi
{
    [Serializable]
    public class ArgumentInfo : InterfaceInfoPart
    {
        public string Type { get; set; }
        public bool IsOptional { get; set; }

        public override string ToString()
        {
            var type = (IsOptional ? "?" : "") + Type;

            return $"{Name} : {type}";
        }
    }
}