using System;

namespace Furesoft.Rpc.Mmf.InformationApi.Attributes
{
    public class ThrowsExceptionAttribute : Attribute
    {
        public string Exception { get; set; }

        public ThrowsExceptionAttribute(string exc)
        {
            Exception = exc;
        }
        public ThrowsExceptionAttribute(Type t)
        {
            Exception = t.Name;
        }
    }
}