using System;
using System.Collections.Generic;
using System.Text;

namespace GameSolver.NET.Tests.Matrix.Data
{
    internal static class TwoPlayerData
    {
        public const string ShouldThrowNotSame1 = @"1,2,3
1,2,3";

        public const string ShouldThrowNotSame2 = @"1,2
1,2";

        public const string ShouldThrowNotRectangular1 = @"1,2,3
1,2";

        public const string ShouldThrowNotRectangular2 = @"1,2,3
1,2";

        public const string ShouldThrowNotRectangular3 = @"1,2
1,2,3";

        public const string ShouldThrowNotRectangular4 = @"1,2
1,2,3";

        public const string ShouldThrowTooSmall = "1";
    }
}
