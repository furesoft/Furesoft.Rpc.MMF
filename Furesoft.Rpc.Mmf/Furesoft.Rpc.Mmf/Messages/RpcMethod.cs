using System.Collections.Generic;
using System.Windows.Markup;

namespace Furesoft.Rpc.Mmf.Messages
{
    [ContentProperty("Args")]
    public class RpcMethod
    {
        public string Interface { get; set; }
        public string Name { get; set; }
        public List<object> Args { get; set; } = new List<object>();
    }
}