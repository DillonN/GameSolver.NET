using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameSolver.NET.Matrix.Solvers
{
    public class TwoPlayerSolver : Solver
    {
        public int Player1Actions { get; private set; }
        public int Player2Actions { get; private set; }

        protected TwoPlayerSolver()
        { }

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

            Player1Actions = Matrices[0].Length;

            if (Player1Actions < 2)
                throw new ArgumentException("Matrix must be at least 2x2");
            if (Player1Actions != Matrices[1].Length)
                throw new ArgumentException("Matrices must have same row count!");

            Player2Actions = Matrices[0][0].Length;

            if (Player2Actions < 2)
                throw new ArgumentException("Matrix must be at least 2x2");

            if (Matrices.Any(matrix => matrix.Any(row => Player2Actions != row.Length)))
            {
                throw new ArgumentException("Matrices must be rectangular!");
            }
        }
    }
}
