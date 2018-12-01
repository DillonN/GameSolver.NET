using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GameSolver.NET.Matrix.Solvers;
using GameSolver.NET.Tests.Matrix.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameSolver.NET.Tests.Matrix.Solvers
{
    [TestClass]
    public class TwoPlayerTest
    {
        [TestMethod]
        public void TestGame1()
        {
            var t = new TwoPlayerSolver(TwoPlayerData.TestGame1A, TwoPlayerData.TestGame1B);

            var s = t.BruteForceSolutions().ToList();

            Assert.AreEqual(1, s.Count);
            Assert.AreEqual(5, s[0].P1Result);
            Assert.AreEqual(5, s[0].P2Result);
            Assert.AreEqual(1, s[0].P1Action);
            Assert.AreEqual(1, s[0].P2Action);
        }

        [TestMethod]
        public void TestGame2()
        {
            var t = new TwoPlayerSolver(TwoPlayerData.TestGame2A, TwoPlayerData.TestGame2B);

            var s = t.BruteForceSolutions().ToList();

            Assert.AreEqual(5, s.Count);

            foreach (var x in s)
            {
                if (x.P1Action == 1 && x.P2Action == 2)
                {
                    Assert.AreEqual(-4, x.P1Result);
                    Assert.AreEqual(0, x.P2Result);
                }
                else if ((x.P1Action == 3 && x.P2Action == 3) ||
                         (x.P1Action == 4 && x.P2Action == 1) ||
                         (x.P1Action == 5 && (x.P2Action == 1 || x.P2Action == 3)))
                {
                    Assert.AreEqual(0, x.P1Result);
                    Assert.AreEqual(0, x.P2Result);
                }
                else
                {
                    Assert.Fail("Unexpected solution!");
                }
            }

            s.Sort();

            var first = s.First();

            Assert.AreEqual(1, first.P1Action);
            Assert.AreEqual(2, first.P2Action);

            foreach (var x in s.Where(i => i != first))
            {
                Assert.IsTrue(first < x);
            }
        }

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
