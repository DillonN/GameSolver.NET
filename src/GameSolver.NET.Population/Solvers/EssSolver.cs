using GameSolver.NET.Common.Models;
using GameSolver.NET.Matrix.Solvers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GameSolver.NET.Tests.Population")]

namespace GameSolver.NET.Population.Solvers
{
    /// <summary>
    /// This class can solve ESS equilibrium problems.
    /// It is not finished but is working in a limited number of examples.
    /// It is not included in the report, but could be expanded in the future.
    /// </summary>
    public class EssSolver
    {
        private readonly TwoPlayerSolver _2PSolver;

        public EssSolver(string matrix)
        {
            _2PSolver = new TwoPlayerSolver(matrix, matrix);

            if (_2PSolver.P1Actions != _2PSolver.P2Actions)
                throw new ArgumentException("Must be a square matrix!", nameof(matrix));
        }

        public IEnumerable<P2PureSolution> GetEquilibriums()
        {
            var nes = _2PSolver.BruteForceSolutions();

            foreach (var ne in nes)
            {
                var ess = true;
                for (var i = 0; i < _2PSolver.P1Actions; i++)
                {
                    // The core logic that separates ESS from normal NE
                    // Known as ESS-2 in the notess
                    if (i != ne.P1Action - 1 &&
                        _2PSolver.P1Matrix[i][ne.P1Action - 1] == ne.P1Result &&
                        _2PSolver.P1Matrix[ne.P1Action - 1][i] >= _2PSolver.P1Matrix[i][i])
                    {
                        ess = false;
                    }
                }

                if (ess) yield return ne;
            }
        }
    }
}
