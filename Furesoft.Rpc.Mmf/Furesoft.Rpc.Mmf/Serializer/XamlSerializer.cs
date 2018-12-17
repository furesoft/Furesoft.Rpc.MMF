using System.IO;
using System.Xaml;

namespace Furesoft.Rpc.Mmf.Serializer
{
    public class XamlSerializer : RpcSerializer
    {
        public override RpcMessage Deserialize(byte[] data)
        {
            try
            {
                return (RpcMessage)XamlServices.Load(new MemoryStream(data));
            }
            catch { }

            return null;
        }

        public override byte[] Serialize(RpcMessage msg)
        {
            var ms = new MemoryStream();
            XamlServices.Save(ms, msg);

            return ms.ToArray();
        }
    }
}