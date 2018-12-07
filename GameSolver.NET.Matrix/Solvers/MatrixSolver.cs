using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("GameSolver.NET.Tests.Matrix")]

namespace GameSolver.NET.Matrix.Solvers
{
    public abstract class MatrixSolver
    {
        protected readonly IReadOnlyList<IReadOnlyList<double>>[] Matrices;

        protected MatrixSolver(params IReadOnlyList<IReadOnlyList<double>>[] matrices)
        {
            Matrices = matrices;
        }

        protected MatrixSolver(params string[] matrices)
            : this((IEnumerable<string>) matrices)
        { }

        protected MatrixSolver(IEnumerable<string> matrices)
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
