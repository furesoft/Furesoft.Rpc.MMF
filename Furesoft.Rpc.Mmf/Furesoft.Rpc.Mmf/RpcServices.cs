using Furesoft.Rpc.Mmf.Serializer;

namespace Furesoft.Rpc.Mmf
{
    public static class RpcServices
    {
        public static RpcMessage Deserialize(byte[] src)
        {
            return new BinarySerializer().Deserialize(src);
        }

        public static byte[] Serialize(RpcMessage m)
        {
            return new BinarySerializer().Serialize(m);
        }
    }
}