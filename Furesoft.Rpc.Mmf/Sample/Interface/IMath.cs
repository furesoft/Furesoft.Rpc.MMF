using Furesoft.Rpc.Mmf;
using Furesoft.Rpc.Mmf.InformationApi.Attributes;
using System;
using System.ComponentModel;

namespace Interface
{
    public interface IMath
    {
        int Add(int x, int y);

        void MethodWithException();
    }
}