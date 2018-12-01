using GameSolver.NET.Common;
using GameSolver.NET.Extensions;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSolver.NET.Matrix.Solvers
{
    public class TwoPlayerZeroSum : TwoPlayerSolver
    {
        private IReadOnlyList<IReadOnlyList<double>> Matrix => Matrices[0];

        public TwoPlayerZeroSum(double[][] matrix)
            : base(matrix, NegativeMatrix(matrix))
        {
            //_matrix = new IEnumerable<double>[2];
            //_matrix[0] = matrix[0];
            //_matrix[1] = matrix[1];
        }

        // Using static instead of constructor so we don't have to parse the matrix twice
        public static TwoPlayerZeroSum Parse(string matrix)
        {
            return new TwoPlayerZeroSum(ParseMatrix(matrix));
        }

        public double CostForPureActions(int player, int u1, int u2)
        {
            if (player > 1)
                throw new ArgumentOutOfRangeException(nameof(player));
            if (u1 > P1Actions - 1)
                throw new ArgumentOutOfRangeException(nameof(u1));
            if (u2 > P2Actions - 1)
                throw new ArgumentOutOfRangeException(nameof(u2));

            return Matrices[player][u1][u2];
        }

        public double CostForMixedActions(int player, double[] x1, double[] x2)
        {
            if (x1.Length > P1Actions)
                throw new ArgumentOutOfRangeException(nameof(x1));
            if (x2.Length > P2Actions)
                throw new ArgumentOutOfRangeException(nameof(x2));

            if (x1.Any(d => d > 1 | d < 0))
                throw new ArgumentOutOfRangeException(nameof(x1));
            if (x2.Any(d => d > 1 | d < 0))
                throw new ArgumentOutOfRangeException(nameof(x2));

            var cost = 0d;

            for (var i = 0; i < x1.Length; i++)
            {
                var rowCost = x2.Select((t, j) => t * CostForPureActions(player, i, j)).Sum();

                cost += x1[i] * rowCost;
            }

            return cost;
        }
        
        public IEnumerable<TwoPlayerZSSolution> MinMaxSolution()
        {
            var p1Actions = new List<int>();
            var p1Security = double.MaxValue;
            for (var i = 0; i < P1Actions; i++)
            {
                var max = Matrix[i].Max();
                if (max == p1Security)
                {
                    p1Actions.Add(i);
                }
                else if (max < p1Security)
                {
                    p1Actions.Clear();
                    p1Actions.Add(i);
                    p1Security = max;
                }
            }


            var p2Actions = new List<int>();
            var p2Security = double.MinValue;
            for (var i = 0; i < P2Actions; i++)
            {
                var min = Matrix.Min(c => c[i]);

                if (min == p2Security)
                {
                    p2Actions.Add(i);
                }
                else if (min > p2Security)
                {
                    p2Actions.Clear();
                    p2Actions.Add(i);
                    p2Security = min;
                }
            }

            foreach (var p1Action in p1Actions)
            {
                foreach (var p2Action in p2Actions)
                {
                    var result = Matrix[p1Action][p2Action];

                    yield return new TwoPlayerZSSolution(p1Action + 1, p2Action + 1, p1Security, p2Security, result);
                }
            }
        }

        public TwoPlayerZSSolution GetMixedSolution()
        {
            var p1 = StrategyForPlayerEn(1);
            double? p2x = null;
            double? p2y = null;
            double? result = null;
            if (P2Actions == 2)
            {
                var p2 = StrategyForPlayerEn(2);
                p2x = p2.X;
                p2y = p2.Y;

                result = Matrix[0][0] * p1.X * p2x + Matrix[1][0] * (1 - p1.X) * p2x +
                         Matrix[0][1] * p1.X * (1 - p2x) + Matrix[1][1] * (1 - p1.X) * (1 - p2x);
            }

            return new TwoPlayerZSSolution(p1.X, p2x, p1.Y, p2y, result);
        }

        public Point2D StrategyForPlayer(int player)
        {
            // Find best response funcs
            var brfs = new Line2D[P2Actions];

            var maxIndex = 0;
            var maxValue = double.MinValue;

            for (var i = 0; i < P2Actions; i++)
            {
                var p1 = new Point2D(0, BestResponse(0, i));
                var p2 = new Point2D(1, BestResponse(1, i));
                brfs[i] = new Line2D(p1, p2);

                if (!(p1.Y > maxValue)) continue;

                maxValue = p1.Y;
                maxIndex = i;
            }

            // Find intersections
            var ints = new Point2D?[brfs.Length][];
            for (var i = 0; i < brfs.Length; i++)
            {
                ints[i] = new Point2D?[brfs.Length];
                for (var j = 0; j < brfs.Length; j++)
                {
                    // Short circuit to null if on the same line
                    ints[i][j] = i == j ? null : brfs[i].IntersectWith(brfs[j]);
                }
            }

            var topLineIntersections = new List<Point2D> { brfs[maxIndex].StartPoint };
            var currentIndex = maxIndex;

            while (true)
            {
                var nextIntersection = new Point2D(double.MaxValue, double.MinValue);
                var currentCompareIndex = currentIndex;
                for (var i = 0; i < brfs.Length; i++)
                {
                    if ((ints[currentIndex][i]?.X < nextIntersection.X ||
                        ints[currentIndex][i]?.Y > nextIntersection.Y) &&
                        ints[currentIndex][i]?.X > topLineIntersections.Last().X)
                    {
                        nextIntersection = ints[currentIndex][i].Value;
                        currentCompareIndex = i;
                    }
                }

                if (currentCompareIndex == currentIndex)
                    break;

                currentIndex = currentCompareIndex;
                topLineIntersections.Add(nextIntersection);
            }

            if (topLineIntersections.Last().X < 1)
            {
                topLineIntersections.Add(brfs[currentIndex].EndPoint);
            }

            var compY = new Point2DComparerY();
            topLineIntersections.Sort(compY);

            return topLineIntersections[0];
        }

        public Point2D StrategyForPlayerEn(int player)
        {
            var x = BestResponses(player)
                .AsParallel()
                .SelectMany(brf1 => 
                    BestResponses(player, brf1)
                    //.Where(b => b != brf1)
                    .Select(brf1.IntersectWith)
                    .Where(intersect => intersect != null)
                    .Select(intersect => intersect.Value)
                    .Where(intersect => 
                        !BestResponses(player)
                        .Where(b => b != brf1)
                        .Any(b => b.GetYForX(intersect.X) > intersect.Y)
                    )
                )
                .OrderBy(p => p.Y);

            try
            {
                return x.First();
            }
            catch (InvalidOperationException)
            {
                // Min point lies on 0 or 1
                var y0 = BestResponses(player)
                    .AsParallel()
                    .Select(b => b.GetYForX(0))
                    .Max();
                var y1 = BestResponses(player)
                    .AsParallel()
                    .Select(b => b.GetYForX(1))
                    .Max();

                return y0 > y1 ? new Point2D(0, y0) : new Point2D(1, y1);
            }
        }

        private double BestResponse(double x1, int p2Index)
        {
            return (Matrices[0][0][p2Index] - Matrices[0][1][p2Index]) * x1 + Matrices[0][1][p2Index];
        }

        private IEnumerable<Line2D> BestResponses(int player, Line2D? start = null)
        {
            if (player > 2 || player < 1)
                throw new ArgumentOutOfRangeException();
            if (player == 2)
            {
                if (P2Actions != 2)
                    throw new InvalidOperationException();

                yield return new Line2D(new Point2D(0, -Matrix[1][0]), new Point2D(1, -Matrix[0][0]));
                yield return new Line2D(new Point2D(0, -Matrix[1][1]), new Point2D(1, -Matrix[0][1]));
                yield break;
            }

            var started = !start.HasValue;
            using (var e1 = Matrix[0].GetEnumerator())
            using (var e2 = Matrix[1].GetEnumerator())
            {
                while (e1.MoveNext() & e2.MoveNext())
                {
                    var line = new Line2D(new Point2D(0, e2.Current),
                        new Point2D(1, e1.Current));
                    if (!started && line == start)
                    {
                        started = true;
                    }
                    else
                    {
                        yield return line;
                    }
                }
            }
        }

        private static double[][] NegativeMatrix(IReadOnlyList<double[]> matrix)
        {
            var m = new double[matrix.Count][];
            for (var i = 0; i < matrix.Count; i++)
            {
                var row = new double[matrix[i].Length];
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    row[j] = -matrix[i][j];
                }

                m[i] = row;
            }

            return m;
        }

        private static int OtherPlayer(int player)
        {
            return player == 0 ? 1 : 0;
        }
    }
}
