using GameSolver.NET.Common.Models;
using MathNet.Numerics;
using System;

namespace GameSolver.NET.Vector.Solvers
{
    public class ContinuousSolver
    {
        private readonly Func<double[], double> _costFunc;

        private readonly double[] _lowerBound;
        private readonly double[] _upperBound;

        public ContinuousSolver(Func<double[], double> costFunc, double[] lower, double[] upper)
        {
            _costFunc = costFunc;
            _lowerBound = lower;
            _upperBound = upper;
        }

        public P2MixedSolution Solve()
        {
            var dj1 = Differentiate.PartialDerivativeFunc(_costFunc, 0, 1);
            var dj2 = Differentiate.PartialDerivativeFunc(_costFunc, 1, 1);
                
            var min1 = FindMinimum.OfFunctionConstrained()
        }
    }
}
