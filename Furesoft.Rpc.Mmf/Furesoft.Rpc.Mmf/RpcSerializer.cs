namespace Furesoft.Rpc.Mmf
{
    public abstract class RpcSerializer
    {
        public abstract RpcMessage Deserialize(byte[] data);
        public abstract byte[] Serialize(RpcMessage msg);
    }
}