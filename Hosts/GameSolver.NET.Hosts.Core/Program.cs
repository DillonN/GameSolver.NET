using GameSolver.NET.Hosts.Benchmarking;
using System;

namespace GameSolver.NET.Hosts.Core
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var a = new []
            {
                new[] { double.PositiveInfinity, 2, 3, 5 },
                new[] { 2, double.PositiveInfinity, -3, -1 },
                new[] { 3, -3, double.PositiveInfinity, 0 },
                new[] { 5, -1, 0, double.PositiveInfinity }
            };

            var b = new double[4][];
            for (var i = 0; i < 4; i++)
            {
                b[i] = new double[4];
                for (var j = 0; j < 4; j++)
                {
                    if (i == j)
                    {
                        b[i][j] = double.PositiveInfinity;
                    }
                    else
                    {
                        b[i][j] = -a[i][j];
                    }
                }
            }

            var t = new TwoPlayerSolver(a, b);

            var s = t.BruteForceSolutions().ToList();

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
