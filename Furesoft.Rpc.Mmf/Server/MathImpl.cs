using Interface;
using System;
using Furesoft.Rpc.Mmf;

namespace Server
{
    class MathImpl : IMath
    {
        public MathImpl()
        {
            OnIndexChanged = RpcEvent.Register(nameof(OnIndexChanged));
        }

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

        public RpcEvent OnIndexChanged { get; set; }

        public int Add(int x, int y)
        {
            OnIndexChanged["hello: " + (x + y), EventArgs.Empty]();

            return x + y;
        }

        public object MethodWithException()
        {
            return new ArgumentNullException("An Error occured!!");
        }
    }
}