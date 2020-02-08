using System;
using Furesoft.Rpc.Mmf;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new RpcClient("ExampleChannel", null);
            client.Start();

            var interfaces = client.GetInterfaceNames();
            var info = client.GetInfo<IMath>();

            var math = client.Bind<IMath>();

            var p = math.AddPosition(10, 15);
            p = math.TranslatePoint(p);

            p.GetX();

            //math.MethodWithException();
            math.OnIndexChanged += Math_IndexChanged;

            Console.WriteLine("result: " + math.Add(15, 5));

            math[10] = 5;

            Console.WriteLine("Index result: " + math[10]);

            try
            {
                //math.MethodWithException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception has thrown: " + ex.Message);
            }

            Console.ReadLine();
        }

        private static void Math_IndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(sender);
        }
    }
}