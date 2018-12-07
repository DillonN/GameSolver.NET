﻿using MathNet.Spatial.Euclidean;
using System.Collections.Generic;

namespace GameSolver.NET.Common
{
    public class Point2DComparerX : IComparer<Point2D>
    {
        public int Compare(Point2D x, Point2D y)
        {
            return x.X.CompareTo(y.X);
        }
    }

    public class Point2DComparerY : IComparer<Point2D>
    {
        public int Compare(Point2D x, Point2D y)
        {
            return x.Y.CompareTo(y.Y);
        }
    }
}
