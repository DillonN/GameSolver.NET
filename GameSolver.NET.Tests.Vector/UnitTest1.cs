using GameSolver.NET.Vector.Solvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GameSolver.NET.Tests.Vector
{
    [TestClass]
    public class UnitTest1
    {
        private const string Matrix1 = @"-4,0
-1,-1";

        [TestMethod]
        public void Ess1ShouldMatch()
        {
            var t = new EssSolver(Matrix1);
            var ess = t.GetEquilibriums().ToList();
        }
    }
}