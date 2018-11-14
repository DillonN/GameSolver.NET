using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameSolver.NET.Common;

namespace GameSolver.NET.Tests.Common
{
    [TestClass]
    public class Point2DComparerTest
    {
        private readonly List<Point2D> _points = new List<Point2D>
        {
            new Point2D(3, 2),
            new Point2D(5, 8),
            new Point2D(0, 6)
        };

        [TestMethod]
        public void CompareByXShouldSort()
        {
            var compX = new Point2DComparerX();

            var sortedPoints = new List<Point2D>(_points);

            sortedPoints.Sort(compX);

            Assert.AreEqual(_points[2], sortedPoints[0]);
            Assert.AreEqual(_points[0], sortedPoints[1]);
            Assert.AreEqual(_points[1], sortedPoints[2]);
        }

        [TestMethod]
        public void CompareByYShouldSort()
        {
            var compY = new Point2DComparerY();

            var sortedPoints = new List<Point2D>(_points);

            sortedPoints.Sort(compY);

            Assert.AreEqual(_points[0], sortedPoints[0]);
            Assert.AreEqual(_points[2], sortedPoints[1]);
            Assert.AreEqual(_points[1], sortedPoints[2]);
        }
    }
}
