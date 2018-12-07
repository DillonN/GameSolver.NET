using GameSolver.NET.Common.Models;

namespace GameSolver.NET.Common
{
    internal static class P2CompareHelper
    {
        /// <summary>
        /// Helper to compare two two-player solutions and select the better one as higher
        /// </summary>
        public static int Compare(IP2Solution left, IP2Solution right)
        {
            var p1 = left.P1Result.CompareTo(right.P1Result);
            var p2 = left.P2Result.CompareTo(right.P2Result);
            if (p1 == 0 && p2 == 0)
                // NEs are the same
                return 0;
            if ((p1 < 0 || p2 < 0) && p1 <= 0 && p2 <= 0)
                // left is better
                return -1;
            // right is better
            return 1;
        }
    }
}
