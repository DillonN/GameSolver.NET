using MathNet.Spatial.Euclidean;
using System;
using System.Collections.Generic;

namespace GameSolver.NET.Matrix.Models
{
    internal class UnorderedLineComparer : IEqualityComparer<(Line2D, Line2D)>
    {
        public bool Equals((Line2D, Line2D) x, (Line2D, Line2D) y)
        {
            return (x.Item1 == y.Item1 || x.Item1 == y.Item2) &&
                   (x.Item2 == y.Item1 || x.Item2 == y.Item2);
        }

        public int GetHashCode((Line2D, Line2D) obj)
        {
            unchecked
            {
                return obj.GetHashCode() * 397 ^ obj.GetHashCode() * 397;
            }
        }
    }
}
