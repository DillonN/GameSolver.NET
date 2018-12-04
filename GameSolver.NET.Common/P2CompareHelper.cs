using GameSolver.NET.Common.Models;

namespace GameSolver.NET.Common
{
    internal static class P2CompareHelper
    {
        public static int Compare(IP2Solution left, IP2Solution right)
        {
            var p1 = left.P1Result.CompareTo(right.P1Result);
            var p2 = left.P2Result.CompareTo(right.P2Result);
            if (p1 == 0 && p2 == 0)
                return 0;
            if ((p1 < 0 || p2 < 0) && p1 <= 0 && p2 <= 0)
                return -1;
            return 1;
        }
    }
}
