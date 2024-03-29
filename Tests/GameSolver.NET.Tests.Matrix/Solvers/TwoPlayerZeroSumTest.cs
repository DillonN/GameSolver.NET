﻿using GameSolver.NET.Matrix.Solvers;
using GameSolver.NET.Tests.Matrix.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GameSolver.NET.Tests.Matrix.Solvers
{
    [TestClass]
    public class TwoPlayerZeroSumTest
    {
        private const double Delta = 0.00001;
        [TestMethod]
        public void TestGame1()
        {
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame1);
            
            Assert.AreEqual(t.CostForPureActions(0, 0, 0), 2);
            Assert.AreEqual(t.CostForPureActions(1, 0, 0), -2);
            Assert.AreEqual(t.CostForPureActions(0, 1, 2), 5);

            Assert.AreEqual(t.CostForMixedActions(0, new [] { 0.5, 0.5 }, new [] { 0.2, 0.4, 0, 0.4 }), 2.4, Delta);

            var s = t.MixedStrategyForPlayer(1);

            Assert.AreEqual(s.X, 4d / 7, Delta);
            Assert.AreEqual(s.Y, 20d / 7, Delta);
        }

        [TestMethod]
        public void TestGame2()
        {
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame2);

            var s = t.MinMaxSolution().First();

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
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame3);

            var s = t.MinMaxSolution().First();

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
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame4);

            foreach (var s in t.MinMaxSolution())
            {
                Assert.IsFalse(s.IsSaddle);
                Assert.AreEqual(1, s.P1Security);
                Assert.AreEqual(-1, s.P2Security);
            }
        }

        [TestMethod]
        public void TestGame5()
        {
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame5);

            var ss = t.MinMaxSolution().ToList();

            Assert.AreEqual(2, ss.Count);

            foreach (var s in ss)
            {
                Assert.AreEqual(2, s.P1Action);
                Assert.AreEqual(0, s.P2Security);
                Assert.IsFalse(s.IsSaddle);
                if (s.P2Action == 1)
                {
                    Assert.AreEqual(1, s.Result);
                }
                else if (s.P2Action == 3)
                {
                    Assert.AreEqual(0, s.Result);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TestGame6()
        {
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame6);

            var ss = t.MinMaxSolution().ToList();

            Assert.AreEqual(2, ss.Count);

            foreach (var s in ss)
            {
                Assert.AreEqual(3, s.P1Action);
                Assert.AreEqual(3, s.P1Security);
                Assert.AreEqual(1, s.P2Security);
                Assert.IsFalse(s.IsSaddle);
                if (s.P2Action == 1)
                {
                    Assert.AreEqual(2, s.Result);
                }
                else if (s.P2Action == 4)
                {
                    Assert.AreEqual(1, s.Result);
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void TestGame7()
        {
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame7);

            var s = t.GetMixedSolution();

            Assert.AreEqual(1d / 5, s.P1Action);
            Assert.AreEqual(4d / 5 - 3, s.P1Security);

            Assert.AreEqual(1d / 5, s.P2Action);
            Assert.AreEqual(1d / 5 + 2, s.P2Security);

            Assert.AreEqual(-2.2, s.Result);
        }

        [TestMethod]
        public void TestGame8()
        {
            var t = new TwoPlayerZeroSum(TwoPlayerZeroSumData.TestGame8);

            var s = t.GetMixedSolution();

            Assert.AreEqual(2d / 3, s.P1Action);
            Assert.AreEqual(-1, s.P1Security);

            Assert.AreEqual(null, s.P2Action);
            Assert.AreEqual(null, s.P2Security);
        }
    }
}
