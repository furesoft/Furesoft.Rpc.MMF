using Furesoft.Rpc.Mmf;
using System;

namespace Server
{
    public static class Program
    {
        public static void Main()
        {
            var rpc = new RpcServer("ExampleChannel");

            rpc.Bind<IMath>(new MathImpl());

            rpc.Start();

            Console.WriteLine("Service started");

            Console.ReadLine();
        }
    }
}