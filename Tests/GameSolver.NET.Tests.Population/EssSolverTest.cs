using GameSolver.NET.Population.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GameSolver.NET.Tests.Population
{
    [TestClass]
    public class EssSolverTest
    {
        private const string Matrix1 = @"-4,0
-1,-1";

        [TestMethod]
        public void Ess1ShouldMatch()
        {
            var t = new EssSolver(Matrix1);
            var ess = t.GetEquilibriums().ToList();

            Assert.AreEqual(2, ess.Count);

            foreach (var e in ess)
            {
                Assert.IsTrue((e.P1Action == 1 && e.P2Action == 1) || (e.P1Action == 2 && e.P2Action == 2));
            }
        }
    }
}
