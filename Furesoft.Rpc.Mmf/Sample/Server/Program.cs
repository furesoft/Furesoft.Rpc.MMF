using Furesoft.Rpc.Mmf;
using Interface;
using System;

namespace Server
{
    public static class Program
    {
        public static void Main()
        {
            var rpc = new RpcServer("ExampleChannel");

            rpc.Bind<IMath>(new MathImpl() as IMath);

            rpc.Start();

            Console.WriteLine("Service started");

            Console.ReadLine();
        }
    }
}