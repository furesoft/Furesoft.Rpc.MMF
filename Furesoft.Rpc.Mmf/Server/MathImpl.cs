using Interface;
using System;
using Furesoft.Rpc.Mmf;
using Furesoft.Rpc.Mmf.Auth;
using System.Diagnostics;

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

        [DebuggerStepThrough]
        public object MethodWithException()
        {
            throw new ArgumentNullException("An Error occured!!");
        }

        public Point AddPosition(int x, int y)
        {
            return new Point() { X = x, Y = y };
        }

        public Point TranslatePoint(Point input)
        {
            return new Point { X = -input.X, Y = -input.Y };
        }
    }
}