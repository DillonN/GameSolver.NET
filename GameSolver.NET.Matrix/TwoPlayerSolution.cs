using System;
using System.Collections.Generic;
using System.Text;

namespace GameSolver.NET.Matrix
{
    public class TwoPlayerSolution
    {
        public readonly int P1Action;
        public readonly int P2Action;
        public readonly double P1Security;
        public readonly double P2Security;
        public readonly double Result;

        public bool IsSaddle => P1Security == P2Security;

        public TwoPlayerSolution(int p1A, int p2A, double p1S, double p2S, double result, bool saddle)
        {
            P1Action = p1A;
            P2Action = p2A;
            P1Security = p1S;
            P2Security = p2S;
            Result = result;
            //IsSaddle = saddle;
        }
    }
}
