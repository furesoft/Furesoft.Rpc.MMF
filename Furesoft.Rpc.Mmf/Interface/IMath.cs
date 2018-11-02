using Furesoft.Rpc.Mmf;
using System;

namespace Interface
{
    public interface IMath
    {
        int Add(int x, int y);
        object MethodWithException();
        int this[int index] { get; set; }
        RpcEvent OnIndexChanged { get; set; }
    }
}