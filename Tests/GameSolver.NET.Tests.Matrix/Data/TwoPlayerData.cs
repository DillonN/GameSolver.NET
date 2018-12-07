using System;
using System.Collections.Generic;
using System.Text;

namespace GameSolver.NET.Tests.Matrix.Data
{
    internal static class TwoPlayerData
    {
        public const string TestGame1A = @"5,0
15,1";
        public const string TestGame1B = @"5,15
0,1";

        public const string TestGame2A = @"8,-4,8
6,1,6
12,0,0
0,0,12
0,4,0";
        public const string TestGame2B = @"4,0,0
0,0,4
0,4,0
0,4,0
0,1,0";

        public const string TestGame3A = @"10,-2
-1,1";
        public const string TestGame3B = @"-5,2
1,-1";

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
