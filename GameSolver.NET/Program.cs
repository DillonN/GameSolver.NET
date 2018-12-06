using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameSolver.NET.Matrix.Solvers;
using GameSolver.NET.Benchmarking;

namespace GameSolver.NET
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (false)
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
