using System;

namespace GameSolver.NET.Common.Models
{
    public readonly struct P2MixedSolution : IP2Solution, IEquatable<P2MixedSolution>
    {
        public readonly double P1Action1;
        public readonly double P2Action1;
        public double P1Result { get; }
        public double P2Result { get; }

        public double P1Action2 => 1 - P1Action1;
        public double P2Action2 => 1 - P2Action1;

        public P2MixedSolution(double p1A, double p2A, double p1R, double p2R)
        {
            P1Action1 = p1A;
            P2Action1 = p2A;
            P1Result = p1R;
            P2Result = p2R;
        }

        public int CompareTo(IP2Solution other)
        {
            return P2CompareHelper.Compare(this, other);
        }

        public int CompareTo(object other)
        {
            return other is IP2Solution sol ? CompareTo(sol) : throw new InvalidOperationException();
        }

        public static bool operator <(P2MixedSolution left, IP2Solution right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(P2MixedSolution left, IP2Solution right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(P2MixedSolution left, IP2Solution right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(P2MixedSolution left, IP2Solution right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(P2MixedSolution left, P2MixedSolution right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(P2MixedSolution left, P2MixedSolution right)
        {
            return !left.Equals(right);
        }

        public bool Equals(P2MixedSolution other)
        {
            return P1Action1.Equals(other.P1Action1) && P2Action1.Equals(other.P2Action1) && 
                   P1Result.Equals(other.P1Result) && P2Result.Equals(other.P2Result);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is P2MixedSolution other && Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
