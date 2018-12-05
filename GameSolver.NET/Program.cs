using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameSolver.NET.Matrix.Solvers;
using GameSolver.NET.Common.Models;

namespace GameSolver.NET
{
    internal class Program
    {
        private static Random _r = new Random();

        private static void Main(string[] args)
        {
            var se = new Stopwatch();
            var s = new Stopwatch();

            for (var i = 1; i < 5; i++)
            {
                var length = (int)Math.Pow(10, i);
                var ts = new TimeSpan();
                var tse = new TimeSpan();
                for (var j = 0; j < 10; j++)
                {
                    var m = RandomColumns(length).ToArray();
                    var n = RandomColumns(length).ToArray();

                    var coop = new CooperativePreferenceMatrix(m, n);

                    var t = new TwoPlayerSolver(coop, coop);

                    se.Restart();
                    t.BruteForceSolutions().ToList();
                    se.Stop();
                    tse += se.Elapsed;

                    s.Restart();
                    //t.StrategyForPlayer(0);
                    s.Stop();
                    ts += s.Elapsed;
                }

                Console.WriteLine($"Length {length} completed in {tse / 10} (enum) {ts / 10} (array)");
            }
                
            Console.ReadKey();
        }

        private static IEnumerable<double> RandomColumns(int amount)
        {
            for (var i = 0; i < amount; i++)
                yield return _r.NextDouble();
        }
    }
}
