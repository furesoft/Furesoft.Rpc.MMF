﻿using System;
using Furesoft.Rpc.Mmf;
using Interface;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RpcClient("ExampleChannel");
            client.Start();

            var math = client.Bind<IMath>();

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