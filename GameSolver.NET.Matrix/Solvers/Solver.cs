﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSolver.NET.Matrix.Solvers
{
    public abstract class Solver
    {
        protected double[][][] Matrices;

        protected Solver(params double[][][] matrices)
        {
            // Check inner arrays are rectangular with same dimensions
            
            for (var i = 0; i < matrices.Length; i++)
            {

            }
            Matrices = matrices;
        }

        protected Solver(params string[] matrices)
            : this((IEnumerable<string>) matrices)
        { }

        protected Solver()
        { }

        protected Solver(IEnumerable<string> matrices)
        {
            Matrices = matrices.Select(ParseMatrix).ToArray();
        }

        protected static double[][] ParseMatrix(string matrix)
        {
            var rows = matrix.Split('\n');
            var m = new double[rows.Length][];
            for (var j = 0; j < rows.Length; j++)
            {
                m[j] = rows[j].Split(',').Select(double.Parse).ToArray();
            }

            return m;
        }
    }
}
