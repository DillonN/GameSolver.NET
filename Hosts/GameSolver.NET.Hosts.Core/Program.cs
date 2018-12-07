using GameSolver.NET.Hosts.Benchmarking;
using System;

namespace GameSolver.NET.Hosts.Core
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (true)
            {
                Console.WriteLine("Pure:");

                foreach (var x in Benchmark.Pure())
                {
                    Console.Write("\t");
                    Console.WriteLine(x);
                }
            }
            else
            {
                Console.WriteLine("Mixed:");

                foreach (var x in Benchmark.Mixed())
                {
                    Console.Write("\t");
                    Console.WriteLine(x);
                }
            }
                
            Console.ReadKey();
        }
    }
}
