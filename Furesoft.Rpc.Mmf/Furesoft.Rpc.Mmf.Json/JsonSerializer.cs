using System.Text;
using Newtonsoft.Json;

namespace Furesoft.Rpc.Mmf.Json
{
    public class JsonSerializer : RpcSerializer
    {
        public override RpcMessage Deserialize(byte[] data)
        {
            return JsonConvert.DeserializeObject<RpcMessage>(Encoding.ASCII.GetString(data));
        }

        public override byte[] Serialize(RpcMessage msg)
        {
            string json = JsonConvert.SerializeObject(msg);

            return Encoding.ASCII.GetBytes(json);
        }
    }
}