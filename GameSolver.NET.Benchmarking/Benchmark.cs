using GameSolver.NET.Matrix.Solvers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GameSolver.NET.Benchmarking
{
    public static class Benchmark
    {
        private static Random _r = new Random(65846);

        public static IEnumerable<string> Pure(int powers = 5)
        {
            return Bench(PureImpl, powers);
        }

        public static IEnumerable<string> Mixed(int powers = 5)
        {
            return Bench(MixedImpl, powers);
        }

        private static IEnumerable<string> Bench(Func<int, (TimeSpan, TimeSpan, int, long)> func, int powers)
        {
            var s = new Stopwatch();

            for (var i = 1; i < powers; i++)
            {
                var length = (int) Math.Pow(10, i);

                var tsReal = new TimeSpan();
                var tsCpu = new TimeSpan();
                var count = 0;

                var mem = 0l;
                var me = Process.GetCurrentProcess();

                for (var j = 0; j < 10; j++)
                {
                    var x = func(length);

                    tsReal += x.Item1;
                    tsCpu += x.Item2;
                    count += x.Item3;

                    if (j == 0)
                    {
                        mem = x.Item4 >> 10;
                    }
                }

                tsReal = new TimeSpan(tsReal.Ticks / 10);
                tsCpu = new TimeSpan(tsCpu.Ticks / 10);

                yield return $"Length {length} completed in {tsReal} (real) {tsCpu} (cpu) {count / 10d} {mem}kB / {(length * length * 8) >> 10}";
            }
        }

        private static (TimeSpan, TimeSpan, int, long) PureImpl(int length)
        {
            var s = new Stopwatch();
            var me = Process.GetCurrentProcess();

            var m = new double[length][];
            var n = new double[length][];
            var o = RandomColumns(length).ToArray();
            var p = RandomColumns(length).ToArray();
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

        private static (TimeSpan, TimeSpan, int, long) MixedImpl(int length)
        {
            var s = new Stopwatch();
            var me = Process.GetCurrentProcess();

            var m = new double[2][];
            m[0] = RandomColumns(length).ToArray();
            m[1] = RandomColumns(length).ToArray();

            var t = new TwoPlayerZeroSum(m);

            var tCpu = me.TotalProcessorTime;
            s.Start();
            var x = t.StrategyForPlayerEn(1);
            s.Stop();
            tCpu = me.TotalProcessorTime - tCpu;

            return (s.Elapsed, tCpu, 1, me.WorkingSet64);
        }

        private static IEnumerable<double> RandomColumns(int amount)
        {
            for (var i = 0; i < amount; i++)
                yield return _r.Next(100);
        }
    }
}
