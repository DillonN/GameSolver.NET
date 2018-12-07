using GameSolver.NET.Common;
using GameSolver.NET.Extensions;
using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;
using System.Linq;
using GameSolver.NET.Common.Models;

namespace GameSolver.NET.Matrix.Solvers
{
    /// <summary>
    /// Solver for a two-player zero-sum game
    /// </summary>
    public class TwoPlayerZeroSum : TwoPlayerSolver
    {
        private IReadOnlyList<IReadOnlyList<double>> Matrix => Matrices[0];

        public TwoPlayerZeroSum(IReadOnlyList<IReadOnlyList<double>> matrix)
            : base(matrix, NegativeMatrix(matrix))
        { }

        public TwoPlayerZeroSum(string matrix) :
            this(ParseMatrix(matrix))
        { }

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
        
        /// <summary>
        /// Find pure solutions using minmax
        /// Not included in benchmarks
        /// </summary>
        /// <returns></returns>
        // O(n * m)
        public IEnumerable<P2ZSMixedSolution> MinMaxSolution()
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

                    yield return new P2ZSMixedSolution(p1Action + 1, p2Action + 1, p1Security, p2Security, result);
                }
            }
        }

        /// <summary>
        /// Get mixed solution for both players, if matrix is 2x2. Else for player 1
        /// </summary>
        public P2ZSMixedSolution GetMixedSolution()
        {
            var p1 = MixedStrategyForPlayer(1);
            double? p2X = null;
            double? p2Y = null;
            double? result = null;
            if (P2Actions == 2)
            {
                var p2 = MixedStrategyForPlayer(2);
                p2X = p2.X;
                p2Y = p2.Y;

                result = CostForMixed(Matrix, p1.X, p2X.Value);
            }

            return new P2ZSMixedSolution(p1.X, p2X, p1.Y, p2Y, result);
        }
        
        /// <summary>
        /// Find mixed solution for given player. 
        /// </summary>
        // O(n^3)
        public Point2D MixedStrategyForPlayer(int player = 1)
        {
            // Only valid for n x 2 matrices
            if (P1Actions > 2)
                throw new InvalidOperationException("Graphical method only available for nx2 matrices");

            // Get a list of all intersections on the top line
            var x = BestResponses(player)  // All best responses for player
                .AsParallel()  // Run this calculation in parallel
                .SelectMany(brf1 => 
                    BestResponses(player, brf1)  // Select all best responses after the current one
                    .Select(brf1.IntersectWith)  // Select intersections from current and this brf
                    .Where(intersect => intersect != null)  // Skip if no intersection found
                    .Select(intersect => intersect.Value)  
                    .Where(intersect => 
                        !BestResponses(player)  // Iterate over all brfs again
                        .Any(b => b.GetYForX(intersect.X) > intersect.Y)  // Select only intersections that are on the top line
                    )
                )
                .OrderBy(p => p.Y);  // Order by Y value so the first will be the lowest

            try
            {
                // All of the calculations in x are deferred, they will not run until this call
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

        /// <summary>
        /// Enumerate best response functions
        /// </summary>
        /// <param name="player">Player to enumerate for</param>
        /// <param name="start">Start from a given function</param>
        private IEnumerable<Line2D> BestResponses(int player, Line2D? start = null)
        {
            if (player > 2 || player < 1)
                throw new ArgumentOutOfRangeException();
            if (player == 2)
            {
                if (P2Actions != 2)
                    throw new InvalidOperationException();

                // Trivial case for finding P2 solutions in 2x2 matrix
                yield return new Line2D(new Point2D(0, -Matrix[1][0]), new Point2D(1, -Matrix[0][0]));
                yield return new Line2D(new Point2D(0, -Matrix[1][1]), new Point2D(1, -Matrix[0][1]));
                yield break;
            }

            var started = !start.HasValue;
            // Iterate over matrix rows simultaneously
            using (var e1 = Matrix[0].GetEnumerator())
            using (var e2 = Matrix[1].GetEnumerator())
            {
                // Important to use & so both values are moved next
                while (e1.MoveNext() & e2.MoveNext())
                {
                    // The best response function for this column
                    var line = new Line2D(new Point2D(0, e2.Current),
                        new Point2D(1, e1.Current));
                    if (!started && line == start)
                    {
                        // We have found the line to start from, so start returning results on next iteration
                        started = true;
                    }
                    else
                    {
                        // Return BRF
                        yield return line;
                    }
                }
            }
        }

        /// <summary>
        /// Get negative of a matrix
        /// </summary>
        private static double[][] NegativeMatrix(IReadOnlyList<IReadOnlyList<double>> matrix)
        {
            var m = new double[matrix.Count][];
            for (var i = 0; i < matrix.Count; i++)
            {
                var row = new double[matrix[i].Count];
                for (var j = 0; j < matrix[i].Count; j++)
                {
                    row[j] = -matrix[i][j];
                }

                m[i] = row;
            }

            return m;
        }
    }
}
