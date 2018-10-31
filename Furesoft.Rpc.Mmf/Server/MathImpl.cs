using Interface;
using System;

namespace Server
{
    class MathImpl : IMath
    {
        private int mul = 1;
        public int this[int index]
        {
            get
            {
                return index * mul;
            }
            set
            {
                mul = index + value;
            }
        }

        public int Add(int x, int y)
        {
            return x + y;
        }

        public object MethodWithException()
        {
            return new ArgumentNullException("An Error occured!!");
        }
    }
}