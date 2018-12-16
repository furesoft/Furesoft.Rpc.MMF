using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Furesoft.Rpc.Mmf.Serializer
{
    public class BinarySerializer : RpcSerializer
    {
        public override RpcMessage Deserialize(byte[] data)
        {
            var bf = new BinaryFormatter();

            return (RpcMessage)bf.Deserialize(new MemoryStream(data));
        }

        public override byte[] Serialize(RpcMessage msg)
        {
            var bf = new BinaryFormatter();

            var ms = new MemoryStream();
            bf.Serialize(ms, msg);

            return ms.ToArray();
        }
    }
}
