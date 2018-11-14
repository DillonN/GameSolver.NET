using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Spatial.Euclidean;

namespace GameSolver.NET.Extensions
{
    public static class Line2DExt
    {
        public static double Evaluate(this Line2D line, double x)
        {
            return (line.EndPoint.Y - line.StartPoint.Y) / (line.EndPoint.X - line.StartPoint.X) *
                   (x - line.StartPoint.X) + line.StartPoint.Y;
        }
    }
}
