using Furesoft.Rpc.Mmf.Messages;
using System.IO;
using System.Xaml;

namespace Furesoft.Rpc.Mmf
{
    public static class RpcServices
    {
        public static byte[] Serialize(RpcMethod m)
        {
            var ms = new MemoryStream();
            XamlServices.Save(ms, m);

            return ms.ToArray();
        }
        public static RpcMethod Deserialize(byte[] src)
        {
            return (RpcMethod)XamlServices.Load(new MemoryStream(src));
        }
    }
}