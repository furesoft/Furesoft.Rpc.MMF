using Furesoft.Rpc.Mmf.Messages;
using System.IO;
using System.Xaml;

namespace Furesoft.Rpc.Mmf
{
    public static class RpcServices
    {
        public static byte[] Serialize(RpcMessage m)
        {
            var ms = new MemoryStream();
            XamlServices.Save(ms, m);

            return ms.ToArray();
        }
        public static RpcMessage Deserialize(byte[] src)
        {
            return (RpcMessage)XamlServices.Load(new MemoryStream(src));
        }
    }
}