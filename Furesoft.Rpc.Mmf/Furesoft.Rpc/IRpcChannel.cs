using Furesoft.Rpc.Mmf;
using System;

namespace Furesoft.Rpc
{
    public interface IRpcChannel : IDisposable
    {
        object SendMessage(byte[] v);
        event Action<byte[]> MessageRecieved;

        void Start();
    }
}