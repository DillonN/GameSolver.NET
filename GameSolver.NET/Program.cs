using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameSolver.NET.Matrix.Solvers;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

#nullable enable

namespace GameSolver.NET
{
    internal class Program
    {
        private static Random r = new Random();

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
                    var m = new double[2][];
                    m[0] = RandomColumns(length).ToArray();
                    m[1] = RandomColumns(length).ToArray();

                    var t = new TwoPlayerZeroSum(m);

                    se.Restart();
                    t.StrategyForPlayerEn(0);
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
                yield return r.Next(int.MaxValue);
        }
    }
}
