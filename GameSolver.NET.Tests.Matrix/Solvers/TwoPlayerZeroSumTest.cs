using System;
using System.Collections.Generic;
using System.Text;
using GameSolver.NET.Matrix.Solvers;
using GameSolver.NET.Tests.Matrix.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSolver.NET.Tests.Matrix.Solvers
{
    [TestClass]
    public class TwoPlayerZeroSumTest
    {
        private const double Delta = 0.00001;
        [TestMethod]
        public void TestGame1()
        {
            var t = TwoPlayerZeroSum.Parse(TwoPlayerZeroSumData.TestGame1);
            
            Assert.AreEqual(t.CostForPureActions(0, 0, 0), 2);
            Assert.AreEqual(t.CostForPureActions(1, 0, 0), -2);
            Assert.AreEqual(t.CostForPureActions(0, 1, 2), 5);

            Assert.AreEqual(t.CostForMixedActions(0, new [] { 0.5, 0.5 }, new [] { 0.2, 0.4, 0, 0.4 }), 2.4, Delta);

            var s = t.StrategyForPlayer(0);

            Assert.AreEqual(s.X, 4d / 7, Delta);
            Assert.AreEqual(s.Y, 20d / 7, Delta);

            var se = t.StrategyForPlayerEn(0);
            Assert.AreEqual(s, se);
        }

        [TestMethod]
        public void TestGame2()
        {
            var t = TwoPlayerZeroSum.Parse(TwoPlayerZeroSumData.TestGame2);

            var s = t.MinMaxSolution();

            Assert.AreEqual(1, s.Result);
            Assert.AreEqual(3, s.P1Action);
            Assert.AreEqual(2, s.P1Security);
            Assert.AreEqual(1, s.P2Action);
            Assert.AreEqual(0, s.P2Security);
            Assert.IsFalse(s.IsSaddle);
        }

        [TestMethod]
        public void TestGame3()
        {
            var t = TwoPlayerZeroSum.Parse(TwoPlayerZeroSumData.TestGame3);

            var s = t.MinMaxSolution();

            Assert.AreEqual(2, s.Result);
            Assert.AreEqual(2, s.P1Action);
            Assert.AreEqual(2, s.P1Security);
            Assert.AreEqual(2, s.P2Action);
            Assert.AreEqual(2, s.P2Security);
            Assert.IsTrue(s.IsSaddle);
        }

        [TestMethod]
        public void TestGame4()
        {
            var t = TwoPlayerZeroSum.Parse(TwoPlayerZeroSumData.TestGame4);

            var s = t.MinMaxSolution();

            Assert.IsFalse(s.IsSaddle);
        }

        [TestMethod]
        public void TestGame5()
        {
            var t = TwoPlayerZeroSum.Parse(TwoPlayerZeroSumData.TestGame5);

            var s = t.MinMaxSolution();
        }
    }
}
