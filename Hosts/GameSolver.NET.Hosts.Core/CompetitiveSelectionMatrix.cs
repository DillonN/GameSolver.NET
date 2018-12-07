using System;
using System.Collections;
using System.Collections.Generic;

namespace GameSolver.NET.Core
{
    /// <summary>
    /// Example for a custom IReadOnlyList<IReadOnlyList<double>> implementation.
    /// Implements the map/gametype selection game described in Section 3.1.
    /// Using this class, memory used to store the game definition will be on the order of N instead of N^2
    /// This was not used for benchmarking or anything else, and is provided as an example only. Untested.
    /// </summary>
    public class CompetitiveSelectionMatrix : IReadOnlyList<IReadOnlyList<double>>
    {
        // The PerformanceEntry objects are the rows of the NxN cost matrix
        // Thus this list will hold N PerformanceEntry objects, each which correspond
        // To N values.
        private readonly List<PerformanceEntry> _entries;

        /// <summary>
        /// Create a new CompetitiveSelectionMatrix object
        /// </summary>
        /// <param name="p1Perfs">P1's performance cost vector</param>
        /// <param name="p2Perfs">P2's performance cost vector</param>
        // IEnumerable<T> is the base interface for all .NET list-like objects. All it requires is the object
        // holds a number of elements that can be iterated over in e.g. a foreach loop.
        // IReadOnlyList<T> is like IEnumerable<T>, but it has a defined size. Elements can also be accessed
        // by an index
        public CompetitiveSelectionMatrix(IReadOnlyList<double> p1Perfs, IReadOnlyList<double> p2Perfs)
        {
            // Matrix is NxN, so the entries and p2 prefs must match in count
            if (_entries.Count != p2Perfs.Count)
                throw new ArgumentException("Must have same number of entries");

            // Create the PerformanceEntry list 
            // Each entry will hold a REFERENCE to the p1Perfs and p2Perfs lists, NOT a copy. 20 bytes for each
            // in total (8 for each list reference, 4 for index)
            // In a naive implementation, we would calculate all the matrix values now and store a list of N 
            // other lists of N values. That is N^2 values to hold in memory.
            // With this, only 2*N values are held in memory, and PerformanceEntry calculates what the
            // matrix value at a given point should be on the fly
            _entries = new List<PerformanceEntry>(p1Perfs.Count);

            for (var i = 0; i < p1Perfs.Count; i++)
            {
                _entries.Add(new PerformanceEntry(p1Perfs, p2Perfs, i));
            }
        }

        /// <summary>
        /// Gets the row at the index. While return type is IReadOnlyList, we are actually returning
        /// a PerformanceEntry object.
        /// </summary>
        public IReadOnlyList<double> this[int index] => _entries[index];

        /// <summary>
        /// Number of elements (rows) in the matrix
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// The enumerator object wil be used when the rows are iterated over e.g. in a foreach loop
        /// </summary>
        public IEnumerator<IReadOnlyList<double>> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Important class to this optimization that results in significantly less memory
        /// used when solving games of type described in Section 3.1.
        /// The entry holds three objects, whereas in the naive implementation it would hold
        /// an array with N values.
        /// </summary>
        private class PerformanceEntry : IReadOnlyList<double>
        {
            // Reference to P1's cost vector
            private readonly IReadOnlyList<double> _p1Perfs;
            // Reference to P2's cost vector
            private readonly IReadOnlyList<double> _p2Perfs;
            // Row index (index of P1's cost vector). Needed for returning infinity on diagonals.
            private readonly int _index;

            public PerformanceEntry(IReadOnlyList<double> p1Perfs, IReadOnlyList<double> p2Perfs, int index)
            {
                _p1Perfs = p1Perfs;
                _p2Perfs = p2Perfs;
                _index = index;
            }

            /// <summary>
            /// Get the cost for a given spot in the matrix
            /// Since this object represents one row at index "_index", this is equivalent to
            /// getting the value of the "_index"th row at the "index"th column.
            /// If the value is on the diagonal, return infinity. Else return P1's cost total subtracted by P2's.
            /// </summary>
            public double this[int index] => index == _index ? 
                double.PositiveInfinity : _p1Perfs[index] + _p1Perfs[_index] - _p2Perfs[index] - _p2Perfs[_index];

            public int Count => _p1Perfs.Count;

            public IEnumerator<double> GetEnumerator()
            {
                return new PreferenceEnumerator(_p1Perfs[_index], _p2Perfs[_index], _p1Perfs.GetEnumerator(), _p2Perfs.GetEnumerator(), _index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class PreferenceEnumerator : IEnumerator<double>
            {
                private readonly double _p1Cost;
                private readonly double _p2Cost;
                private readonly IEnumerator<double> _p1Enum;
                private readonly IEnumerator<double> _p2Enum;
                private readonly int _index;
                private int _currentIndex;

                public PreferenceEnumerator(double p1Cost, double p2Cost, IEnumerator<double> p1Enum, IEnumerator<double> p2Enum, int index)
                {
                    _p1Cost = p1Cost;
                    _p2Cost = p2Cost;
                    _p1Enum = p1Enum;
                    _p2Enum = p2Enum;
                    _index = index;
                }

                /// <summary>
                /// Current element 
                /// </summary>
                public double Current => _currentIndex == _index ? 
                    double.PositiveInfinity : _p1Cost + _p1Enum.Current - _p2Cost - _p1Enum.Current;

                object IEnumerator.Current => Current;

                public void Dispose()
                {
                    _p1Enum.Dispose();
                    _p2Enum.Dispose();
                }

                /// <summary>
                /// Move to the next element
                /// </summary>
                /// <returns>True iff there is another element</returns>
                public bool MoveNext()
                {
                    _currentIndex++;
                    // Using & instead of && will evaluate both methods even if the first one is false.
                    // && by contrast will short circuit in that case
                    return _p1Enum.MoveNext() & _p2Enum.MoveNext();
                }

                /// <summary>
                /// Resets to before the first element
                /// </summary>
                public void Reset()
                {
                    _currentIndex = -1;
                    _p1Enum.Reset();
                    _p2Enum.Reset();
                }
            }
        }
    }
}
