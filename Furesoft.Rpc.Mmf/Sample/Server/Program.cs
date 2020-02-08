using Furesoft.Rpc.Mmf;
using System;

namespace Server
{
    public class Program
    {
        public static void Main()
        {
            var rpc = new RpcServer("ExampleChannel");
            // rpc.Bootstrapper = ...
            //rpc.BeforeRequest +=..
            //rpc.AfterRequest += ..

            AuthModule.Claims.Add("math:add");

            rpc.Bind<IMath>(new MathImpl());

            rpc.Start();

            Console.WriteLine("Service started");

            Console.ReadLine();
        }
    }
}