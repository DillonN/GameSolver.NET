using GameSolver.NET.Hosts.Benchmarking;
using System;
using System.Linq;
using GameSolver.NET.Matrix.Solvers;

namespace GameSolver.NET.Hosts.Core
{
    /// <summary>
    /// Host for benchmarking on desktop and server platforms
    /// Logic was hardcoded and changed during experiments, logic here not necessarily the same
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // Find solutions for specific example in Section 3.1
            //var a = new []
            //{
            //    new[] { double.PositiveInfinity, 2, 3, 5 },
            //    new[] { 2, double.PositiveInfinity, -3, -1 },
            //    new[] { 3, -3, double.PositiveInfinity, 0 },
            //    new[] { 5, -1, 0, double.PositiveInfinity }
            //};

            //var b = new double[4][];
            //for (var i = 0; i < 4; i++)
            //{
            //    b[i] = new double[4];
            //    for (var j = 0; j < 4; j++)
            //    {
            //        if (i == j)
            //        {
            //            b[i][j] = double.PositiveInfinity;
            //        }
            //        else
            //        {
            //            b[i][j] = -a[i][j];
            //        }
            //    }
            //}

            //var t = new TwoPlayerSolver(a, b);

            //var s = t.BruteForceSolutions().ToList();

            // Begin actual benchmarking

            if (true)
            {
                Console.WriteLine("Pure:");

                foreach (var x in Benchmark.Pure(4))
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
                
            // Do not close the window until a key is pressed
            Console.ReadKey();
        }
    }
}
