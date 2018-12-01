using System;
using System.Collections.Generic;

namespace GameSolver.NET.Matrix
{
    public readonly struct TwoPlayerSolution : IComparable<TwoPlayerSolution>, IComparable, IEquatable<TwoPlayerSolution>
    {
        public readonly int P1Action;
        public readonly int P2Action;
        public readonly double P1Result;
        public readonly double P2Result;

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

        public int CompareTo(TwoPlayerSolution other)
        {
            var p1 = P1Result.CompareTo(other.P1Result);
            var p2 = P2Result.CompareTo(other.P2Result);
            if (p1 == 0 && p2 == 0)
                return 0;
            if ((p1 < 0 || p2 < 0) && p1 <= 0 && p2 <= 0)
                return -1;
            return 1;
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is TwoPlayerSolution other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(TwoPlayerSolution)}");
        }

        public static bool operator <(TwoPlayerSolution left, TwoPlayerSolution right)
        {
            return Comparer<TwoPlayerSolution>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(TwoPlayerSolution left, TwoPlayerSolution right)
        {
            return Comparer<TwoPlayerSolution>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(TwoPlayerSolution left, TwoPlayerSolution right)
        {
            return Comparer<TwoPlayerSolution>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(TwoPlayerSolution left, TwoPlayerSolution right)
        {
            return Comparer<TwoPlayerSolution>.Default.Compare(left, right) >= 0;
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
            unchecked
            {
                var hashCode = P1Action;
                hashCode = (hashCode * 397) ^ P2Action;
                hashCode = (hashCode * 397) ^ P1Result.GetHashCode();
                hashCode = (hashCode * 397) ^ P2Result.GetHashCode();
                return hashCode;
            }
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
