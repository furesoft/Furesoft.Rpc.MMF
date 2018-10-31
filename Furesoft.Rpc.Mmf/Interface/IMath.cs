using System;

namespace Interface
{
    public interface IMath
    {
        int Add(int x, int y);
        object MethodWithException();
        int this[int index] { get; set; }
    }
}