﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GameSolver.NET.Common.Models;

namespace GameSolver.NET.Matrix.Solvers
{
    public class TwoPlayerSolver : MatrixSolver
    {
        public IReadOnlyList<IReadOnlyList<double>> P1Matrix => Matrices[0];
        public IReadOnlyList<IReadOnlyList<double>> P2Matrix => Matrices[1];

        public int P1Actions { get; private set; }
        public int P2Actions { get; private set; }

        public TwoPlayerSolver(IReadOnlyList<IReadOnlyList<double>> matrix1, 
            IReadOnlyList<IReadOnlyList<double>> matrix2)
            : base(matrix1, matrix2)
        {
            CheckArrays();
        }

        public TwoPlayerSolver(string matrix1, string matrix2)
            : base(matrix1, matrix2)
        {
            CheckArrays();
        }

        // O(n * m)
        public IEnumerable<TwoPlayerSolution> BruteForceSolutions()
        {
            var minCols = P2Matrix.AsParallel().Select(r => r.Min()).ToList();
            var minRows = P1Matrix[0].AsParallel().Select((_, i) => P1Matrix.Select(r => r[i]).Min()).ToList();

            for (var i = 0; i < P1Actions; i++)
            {
                for (var j = 0; j < P2Actions; j++)
                {
                    if (P1Matrix[i][j] > minRows[j] || P2Matrix[i][j] > minCols[i])
                    {
                        continue;
                    }

                    yield return new TwoPlayerSolution(i + 1, j + 1, P1Matrix[i][j], P2Matrix[i][j]);
                }
            }

            //return P1Matrix
            //    .AsParallel()
            //    .SelectMany((c, i) => c
            //        .Select((d, j) => new TwoPlayerSolution(i + 1, j + 1, d, P2Matrix[i][j])))
            //    .Where(s => s.P1Result <= minRows[s.P2Action - 1])//1Matrix.Any(r => r[s.P2Action - 1] < s.P1Result))
            //    .Where(s => s.P2Result <= minCols[s.P1Action - 1]);// !P2Matrix[s.P1Action - 1].Any(v => v < s.P2Result));
        }

        public P2MixedSolution Get2x2MixedSolution()
        {
            if (P1Actions != 2 || P2Actions != 2)
                throw new InvalidOperationException("Can only get mixed solutions for 2x2 matrices!");

            var a = P1Matrix[0][0] - P1Matrix[0][1] - P1Matrix[1][0] + P1Matrix[1][1];
            var c1 = P1Matrix[1][1] - P1Matrix[0][1];
            var c2 = P1Matrix[1][1] - P1Matrix[1][0];

            var b = P2Matrix[0][0] - P2Matrix[0][1] - P2Matrix[1][0] + P2Matrix[1][1];
            var d1 = P2Matrix[1][1] - P2Matrix[0][1];
            var d2 = P2Matrix[1][1] - P2Matrix[1][0];

            var p1A1 = d2 / b;
            var p2A1 = c1 / a;

            var p1Result = CostForMixed(P1Matrix, p1A1, p2A1);
            var p2Result = CostForMixed(P2Matrix, p1A1, p2A1);

            return new P2MixedSolution(p1A1, p2A1, p1Result, p2Result);
        }

        private void CheckArrays()
        {
            Debug.Assert(Matrices.Length == 2);

            P1Actions = P1Matrix.Count;

            if (P1Actions < 2)
                throw new ArgumentException("Matrix must be at least 2x2");
            if (P1Actions != Matrices[1].Count)
                throw new ArgumentException("Matrices must have same row count!");

            P2Actions = Matrices[0][0].Count;

            if (P2Actions < 2)
                throw new ArgumentException("Matrix must be at least 2x2");

            if (Matrices.Any(matrix => matrix.Any(row => P2Actions != row.Count)))
            {
                throw new ArgumentException("Matrices must be rectangular!");
            }
        }

        protected static double CostForMixed(IReadOnlyList<IReadOnlyList<double>> matrix, double p1A1, double p2A1)
        {
            return matrix[0][0] * p1A1 * p2A1 + matrix[1][0] * (1 - p1A1) * p2A1 +
                   matrix[0][1] * p1A1 * (1 - p2A1) + matrix[1][1] * (1 - p1A1) * (1 - p2A1);
        }
    }
}
