using System;
using Furesoft.Rpc.Mmf;
using Interface;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = new RpcClient("ExampleChannel", null);
            client.Start();

            var math = client.Bind<IMath>();

            var p = math.Add(10, 15);

            try
            {
                math.MethodWithException();
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