using Interface;
using System;
using Furesoft.Rpc.Mmf;

namespace Server
{
    class MathImpl : IMath
    {
        public MathImpl(RpcServer s)
        {
            this.s = s;
            OnIndexChanged = RpcEvent.Register(s, nameof(OnIndexChanged));
        }

        private int mul = 1;
        private readonly RpcServer s;

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
            OnIndexChanged.Invoke("hello: " + (x + y), EventArgs.Empty);

            return x + y;
        }

        public object MethodWithException()
        {
            return new ArgumentNullException("An Error occured!!");
        }
    }
}