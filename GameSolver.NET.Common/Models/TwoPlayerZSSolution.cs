namespace GameSolver.NET.Common.Models
{
    public class TwoPlayerZSSolution
    {
        public readonly double P1Action;
        public readonly double? P2Action;
        public readonly double P1Security;
        public readonly double? P2Security;
        public readonly double? Result;

        public bool IsSaddle => P1Security == P2Security;

        public TwoPlayerZSSolution(double p1A, double? p2A, double p1S, double? p2S, double? result)
        {
            P1Action = p1A;
            P2Action = p2A;
            P1Security = p1S;
            P2Security = p2S;
            Result = result;
        }
    }
}
