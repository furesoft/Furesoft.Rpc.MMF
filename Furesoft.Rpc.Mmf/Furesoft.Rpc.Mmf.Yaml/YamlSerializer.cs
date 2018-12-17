using System.IO;
using System.Text;

namespace Furesoft.Rpc.Mmf.Yaml
{
    public class YamlSerializer : RpcSerializer
    {
        public override RpcMessage Deserialize(byte[] data)
        {
            var serializer = new SharpYaml.Serialization.Serializer();

            return serializer.Deserialize<RpcMessage>(new MemoryStream(data));
        }

        public override byte[] Serialize(RpcMessage msg)
        {
            var serializer = new SharpYaml.Serialization.Serializer();
            var text = serializer.Serialize(msg);

            return Encoding.ASCII.GetBytes(text);
        }
    }
}