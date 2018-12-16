using System;

namespace Furesoft.Rpc.Mmf.Auth
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthAttribute : Attribute
    {
        public string Claim { get; set; }

        public AuthAttribute(string c)
        {
            Claim = c;
        }
        public AuthAttribute()
        {

        }
    }
}
