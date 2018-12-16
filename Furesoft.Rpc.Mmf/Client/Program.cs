using System;
using System.Diagnostics;
using System.IO;
using Furesoft.Rpc.Mmf;
using Furesoft.Rpc.Mmf.Serializer;
using Interface;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RpcClient("ExampleChannel", null, null);
            client.Start();

            var info = client.GetInfo<IMath>();

            var tmp = Path.GetTempFileName() + ".cs";
            File.WriteAllText(tmp, info.ToString());
            Process.Start(tmp);

            var math = client.Bind<IMath>();

            var p = math.AddPosition(10, 15);

            math.MethodWithException();
            math.OnIndexChanged += Math_IndexChanged;

            Console.WriteLine("result: " + math.Add(15, 5));
            
            math[10] = 5;

            Console.WriteLine("Index result: " + math[10]);
            

            try
            {
                //math.MethodWithException();
            }
            catch(Exception ex) {
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