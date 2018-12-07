using MathNet.Spatial.Euclidean;
using System.Collections.Generic;

namespace GameSolver.NET.Common
{
    /// <summary>
    /// Compare a Point2D by its X value
    /// </summary>
    public class Point2DComparerX : IComparer<Point2D>
    {
        public int Compare(Point2D x, Point2D y)
        {
            return x.X.CompareTo(y.X);
        }
    }

    /// <summary>
    /// Compare a Point2D by its Y value
    /// </summary>
    public class Point2DComparerY : IComparer<Point2D>
    {
        public int Compare(Point2D x, Point2D y)
        {
            return x.Y.CompareTo(y.Y);
        }
    }
}
