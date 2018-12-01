using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameSolver.NET.Matrix.Solvers
{
    public class TwoPlayerSolver : Solver
    {
        private IReadOnlyList<IReadOnlyList<double>> P1Matrix => Matrices[0];
        private IReadOnlyList<IReadOnlyList<double>> P2Matrix => Matrices[1];

        public int P1Actions { get; private set; }
        public int P2Actions { get; private set; }

        public TwoPlayerSolver(double[][] matrix1, double[][] matrix2)
            : base(matrix1, matrix2)
        {
            CheckArrays();
        }

        public TwoPlayerSolver(string matrix1, string matrix2)
            : base(matrix1, matrix2)
        {
            CheckArrays();
        }

        private void CheckArrays()
        {
            Debug.Assert(Matrices.Length == 2);

            P1Actions = P1Matrix.Count;

            if (P1Actions < 2)
                throw new ArgumentException("Matrix must be at least 2x2");
            if (P1Actions != Matrices[1].Length)
                throw new ArgumentException("Matrices must have same row count!");

            P2Actions = Matrices[0][0].Length;

            if (P2Actions < 2)
                throw new ArgumentException("Matrix must be at least 2x2");

            if (Matrices.Any(matrix => matrix.Any(row => P2Actions != row.Length)))
            {
                throw new ArgumentException("Matrices must be rectangular!");
            }
        }
    }
}
