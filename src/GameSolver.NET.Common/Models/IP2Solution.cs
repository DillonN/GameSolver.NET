using System;

namespace GameSolver.NET.Common.Models
{
    public interface IP2Solution : IComparable<IP2Solution>, IComparable
    {
        double P1Result { get; }
        double P2Result { get; }
    }
}
