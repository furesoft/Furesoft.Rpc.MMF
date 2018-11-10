using System.Collections.Generic;
using System.Windows.Markup;

namespace Furesoft.Rpc.Mmf.Messages
{
    [ContentProperty("Args")]
    public class RpcMethod : RpcMessage
    {
        public List<object> Args { get; set; } = new List<object>();
    }
}