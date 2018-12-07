using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameSolver.NET.Matrix.Solvers;

namespace GameSolver.NET.Hosts.Benchmarking
{
    /// <summary>
    /// Class used for benchmarking
    /// Most logic was done while hardcoded, so experiments do not necessarily reflect the logic shown here
    /// </summary>
    public static class Benchmark
    {
        /// <summary>
        /// Random number generator
        /// Do NOT access from multiple threads simultaneously
        /// </summary>
        private static readonly Random R = new Random(65846);

        public static IEnumerable<string> Pure(int powers = 5, int values = 100)
        {
            return Bench(PureImpl, powers, values);
        }

        public static IEnumerable<string> Mixed(int powers = 5, int values = 100)
        {
            return Bench(MixedImpl, powers, values);
        }

        /// <summary>
        /// Run the benchmark
        /// </summary>
        /// <param name="func">Benchmark to run</param>
        /// <param name="powers">Run up to 10^(powers)</param>
        /// <param name="values">K</param>
        /// <returns></returns>
        private static IEnumerable<string> Bench(Func<int, int, (TimeSpan, TimeSpan, int, long)> func, int powers, int values)
        {
            for (var i = 1; i < powers; i++)
            {
                var length = (int) Math.Pow(10, powers);

                // Used to store processing times
                var tsReal = new TimeSpan();
                var tsCpu = new TimeSpan();
                // Number of NEs found
                var count = 0;

                // Memory used
                var mem = 0L;

                // Run benchmark 10 times
                for (var j = 0; j < 10; j++)
                {
                    var x = func(length, values);

                    tsReal += x.Item1;
                    tsCpu += x.Item2;
                    count += x.Item3;

                    if (j == 0)
                    {
                        // Only record memory on first iteration, so previous allocated arrays to not skew results
                        mem = x.Item4 >> 10;
                    }
                }

                // Average the times from 10 runs
                tsReal = new TimeSpan(tsReal.Ticks / 10);
                tsCpu = new TimeSpan(tsCpu.Ticks / 10);

                // Return a description of benchmark results
                yield return $"Length {length} completed in {tsReal} (real) {tsCpu} (cpu) {count / 10d} {mem}kB / {(length * length * 8) >> 10}";
            }
        }

        /// <summary>
        /// Benchmark pure solution from Section 3.1.
        /// </summary>
        /// <param name="length">N</param>
        /// <param name="values">K</param>
        /// <returns>Tuple of real-time, CPU time, NEs found, and memory used</returns>
        private static (TimeSpan, TimeSpan, int, long) PureImpl(int length, int values)
        {
            var s = new Stopwatch();
            var me = Process.GetCurrentProcess();

            var m = new double[length][];
            var n = new double[length][];
            var o = RandomColumns(length, values).ToArray();
            var p = RandomColumns(length, values).ToArray();
            for (var k = 0; k < length; k++)
            {
                m[k] = new double[length];
                n[k] = new double[length];
                for (var l = 0; l < length; l++)
                {
                    if (l == k)
                    {
                        m[k][l] = double.PositiveInfinity;
                        n[k][l] = double.PositiveInfinity;
                    }
                    else
                    {
                        m[k][l] = o[k] + o[l] - p[k] - p[l];
                        n[k][l] = -m[k][l];
                    }
                }
            }

            var t = new TwoPlayerSolver(m, n);

            var tCpu = me.TotalProcessorTime;
            s.Start();
            var x = t.BruteForceSolutions().ToList();
            s.Stop();
            tCpu = me.TotalProcessorTime - tCpu;

            return (s.Elapsed, tCpu, x.Count, me.WorkingSet64);
        }

        /// <summary>
        /// Benchmark mixed solution from Section 3.2.
        /// </summary>
        /// <param name="length">N</param>
        /// <param name="values">K</param>
        /// <returns>Tuple of real-time, CPU time, NEs found, and memory used</returns>
        private static (TimeSpan, TimeSpan, int, long) MixedImpl(int length, int values)
        {
            var s = new Stopwatch();
            var me = Process.GetCurrentProcess();

            var m = new double[2][];
            m[0] = RandomColumns(length, values).ToArray();
            m[1] = RandomColumns(length, values).ToArray();

            var t = new TwoPlayerZeroSum(m);

            var tCpu = me.TotalProcessorTime;
            s.Start();
            var x = t.MixedStrategyForPlayer(1);
            s.Stop();
            tCpu = me.TotalProcessorTime - tCpu;

            return (s.Elapsed, tCpu, 1, me.WorkingSet64);
        }

        // Helper to create random arrays
        private static IEnumerable<double> RandomColumns(int amount, int values)
        {
            for (var i = 0; i < amount; i++)
                yield return R.Next(values);
        }
    }
}
