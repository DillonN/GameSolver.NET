using System;
using GameSolver.NET.Matrix.Solvers;
using GameSolver.NET.Tests.Matrix.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSolver.NET.Tests.Matrix.Solvers
{
    [TestClass]
    public class TwoPlayerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShouldThrowNotSame()
        {
            var t = new TwoPlayerSolver(TwoPlayerData.ShouldThrowNotSame1, TwoPlayerData.ShouldThrowNotSame2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShouldThrowNotRectangular()
        {
            var t = new TwoPlayerSolver(TwoPlayerData.ShouldThrowNotRectangular1, TwoPlayerData.ShouldThrowNotRectangular2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShouldThrowNotRectangular2()
        {
            var t = new TwoPlayerSolver(TwoPlayerData.ShouldThrowNotRectangular3, TwoPlayerData.ShouldThrowNotRectangular4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestShouldThrowTooSmall()
        {
            var t = new TwoPlayerSolver(TwoPlayerData.ShouldThrowTooSmall, TwoPlayerData.ShouldThrowTooSmall);
        }
    }
}
