using Furesoft.Rpc.Mmf;
using Furesoft.Rpc.Mmf.Auth;
using Furesoft.Rpc.Mmf.Serializer;
using Interface;
using System;
using System.Threading;

namespace Server
{
    
    public class Program
    {
        public static void Main()
        {
            var rpc = new RpcServer("ExampleChannel", new TestBoot());
            // rpc.Bootstrapper = ...
            //rpc.BeforeRequest +=..
            //rpc.AfterRequest += ..

            AuthModule.Claims.Add("math:sub");

            rpc.Bind<IMath>(new MathImpl());
            
            rpc.Start();

            Console.ReadLine();
        }
    }

    internal class TestBoot : RpcBootstrapper
    {
        public override void Boot()
        {
            AuthModule.Enable(this);
            base.Boot();
        }
    }
}