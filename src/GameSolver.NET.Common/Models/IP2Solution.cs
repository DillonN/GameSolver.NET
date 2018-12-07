using System;

namespace GameSolver.NET.Common.Models
{
    /// <summary>
    /// Contract for a two player solution model.
    /// </summary>
    public interface IP2Solution : IComparable<IP2Solution>, IComparable
    {
        double P1Result { get; }
        double P2Result { get; }
    }
}
