using GameSolver.NET.Common.Models;
using GameSolver.NET.Matrix.Solvers;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameSolver.NET.Vector.Solvers
{
    public class EssSolver
    {
        private readonly TwoPlayerSolver _2PSolver;

        public EssSolver(string matrix)
        {
            _2PSolver = new TwoPlayerSolver(matrix, matrix);

            if (_2PSolver.P1Actions != _2PSolver.P2Actions)
                throw new ArgumentException("Must be a square matrix!", nameof(matrix));
        }

        public IEnumerable<IP2Solution> GetEquilibriums()
        {
            var nes = _2PSolver.BruteForceSolutions();
            //if (_2PSolver.P1Actions == 2)
            //{
            //    nes = nes.Concat(new IP2Solution[] { _2PSolver.Get2x2MixedSolution() });
            //}

            foreach (var ne in nes)
            {
                var ess = true;
                for (var i = 0; i < _2PSolver.P1Actions; i++)
                {
                    if (_2PSolver.P1Matrix[i][ne.P1Action] == ne.P1Result &&
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
