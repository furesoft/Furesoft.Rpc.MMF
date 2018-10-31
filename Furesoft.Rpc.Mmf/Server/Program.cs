using Furesoft.Rpc.Mmf;
using Interface;
using System;

namespace Server
{
    
    public class Program
    {
        public static void Main()
        {
            var rpc = new RpcServer("ExampleChannel");

            rpc.Bind<IMath>(new MathImpl());
            rpc.Start();

            Console.ReadLine();
        }
    }
}