using Furesoft.Rpc.Mmf;
using Interface;
using System;
using System.Threading;

namespace Server
{
    
    public class Program
    {
        public static void Main()
        {
            var rpc = new RpcServer("ExampleChannel");

            rpc.Bind<IMath>(new MathImpl(rpc));
            
            rpc.Start();

            Thread.Sleep(5000);

            Console.ReadLine();
        }
    }
}