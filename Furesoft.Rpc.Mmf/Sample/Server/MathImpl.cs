using Interface;
using System;

namespace Server
{
    internal class MathImpl : IMath
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public void MethodWithException()
        {
            throw new NotImplementedException();
        }
    }
}