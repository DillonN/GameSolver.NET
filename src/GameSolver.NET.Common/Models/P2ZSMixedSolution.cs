namespace GameSolver.NET.Common.Models
{
    /// <summary>
    /// Two player zero-sum mixed solution.
    /// </summary>
    public readonly struct P2ZSMixedSolution
    {
        public readonly double P1Action;
        // Null if no actions calculated for P2
        public readonly double? P2Action;
        public readonly double P1Security;
        // Null if no actions calculated for P2
        public readonly double? P2Security;
        // Null if no actions calculated for P2
        public readonly double? Result;

        public bool IsSaddle => P1Security == P2Security;

        public P2ZSMixedSolution(double p1A, double? p2A, double p1S, double? p2S, double? result)
        {
            P1Action = p1A;
            P2Action = p2A;
            P1Security = p1S;
            P2Security = p2S;
            Result = result;
        }
    }
}
