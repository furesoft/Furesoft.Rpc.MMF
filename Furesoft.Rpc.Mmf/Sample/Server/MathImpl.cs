using System;
using Furesoft.Rpc.Mmf;
using System.Diagnostics;
using Interface;

namespace Server
{
    internal class MathImpl : IMath
    {
        public RpcEvent OnIndexChanged { get; set; }

        public MathImpl()
        {
            OnIndexChanged = RpcEvent.Register(nameof(OnIndexChanged));
        }

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
            OnIndexChanged["hello: " + (x + y), EventArgs.Empty]();

            return x + y;
        }

        public Point AddPosition(int x, int y)
        {
            return new Point() { X = x, Y = y };
        }

        [DebuggerStepThrough]
        public object MethodWithException()
        {
            throw new ArgumentNullException("An Error occured!!");
        }

        public Point TranslatePoint(Point input)
        {
            return new Point { X = -input.X, Y = -input.Y };
        }

        private int mul = 1;
    }
}