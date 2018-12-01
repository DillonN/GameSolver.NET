using GameSolver.NET.Matrix.Models;
using System;

namespace GameSolver.NET.Matrix
{
    public readonly struct TwoPlayerSolution : IP2Solution, IEquatable<TwoPlayerSolution>
    {
        public readonly int P1Action;
        public readonly int P2Action;
        public double P1Result { get; }
        public double P2Result { get; }

        public TwoPlayerSolution(int p1A, int p2A, double p1R, double p2R)
        {
            P1Action = p1A;
            P2Action = p2A;
            P1Result = p1R;
            P2Result = p2R;
        }

        public override string ToString()
        {
            return $"Action ({P1Action}, {P2Action}) with cost ({P1Result}, {P2Result})";
        }

        public int CompareTo(IP2Solution other)
        {
            return P2CompareHelper.Compare(this, other);
        }

        public int CompareTo(object other)
        {
            return other is IP2Solution sol ? CompareTo(sol) : throw new InvalidOperationException();
        }

        public static bool operator <(TwoPlayerSolution left, IP2Solution right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(TwoPlayerSolution left, IP2Solution right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(TwoPlayerSolution left, IP2Solution right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(TwoPlayerSolution left, IP2Solution right)
        {
            return left.CompareTo(right) >= 0;
        }

        public bool Equals(TwoPlayerSolution other)
        {
            return P1Action == other.P1Action && P2Action == other.P2Action && 
                   P1Result.Equals(other.P1Result) && P2Result.Equals(other.P2Result);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TwoPlayerSolution other && Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TwoPlayerSolution left, TwoPlayerSolution right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TwoPlayerSolution left, TwoPlayerSolution right)
        {
            return !left.Equals(right);
        }
    }
}
