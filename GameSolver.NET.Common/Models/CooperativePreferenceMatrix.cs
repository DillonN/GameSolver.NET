using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameSolver.NET.Common.Models
{
    public class CooperativePreferenceMatrix : IReadOnlyList<IReadOnlyList<double>>
    {
        //private readonly IReadOnlyList<double> _p1Prefs;
        //private readonly IReadOnlyList<double> _p2Prefs;

        private readonly IReadOnlyList<PreferenceEntry> _entries;

        public CooperativePreferenceMatrix(IReadOnlyList<double> p1Prefs, IReadOnlyList<double> p2Prefs)
        {
            if (p1Prefs.Count != p2Prefs.Count)
                throw new ArgumentException("Must have same number of entries");

            //_p1Prefs = p1Prefs;
            //_p2Prefs = p2Prefs;

            _entries = p1Prefs.Select(p => new PreferenceEntry(p, p2Prefs)).ToArray();
        }

        public IReadOnlyList<double> this[int index] => _entries[index];

        public int Count => _entries.Count;

        public IEnumerator<IReadOnlyList<double>> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //private class CoopPreferenceEnumerator : IEnumerator<IReadOnlyList<double>>
        //{
        //    private readonly IEnumerator<double> _p1Prefs;
        //    private readonly IReadOnlyList<double> _p2Prefs;

        //    public CoopPreferenceEnumerator(IEnumerator<double> p1Prefs, IReadOnlyList<double> p2Prefs)
        //    {
        //        _p1Prefs = p1Prefs;
        //        _p2Prefs = p2Prefs;
        //    }

        //    public IReadOnlyList<double> Current => new PreferenceEntry(_p1Prefs.Current, _p2Prefs);

        //    object IEnumerator.Current => Current;

        //    public void Dispose()
        //    {
        //        _p1Prefs.Dispose();
        //    }

        //    public bool MoveNext()
        //    {
        //        return _p1Prefs.MoveNext();
        //    }

        //    public void Reset()
        //    {
        //        _p1Prefs.Reset();
        //    }
        //}

        private class PreferenceEntry : IReadOnlyList<double>
        {
            private readonly double _rowCost;
            private readonly IReadOnlyList<double> _row;

            public PreferenceEntry(double rowCost, IReadOnlyList<double> row)
            {
                _rowCost = rowCost;
                _row = row;
            }

            public double this[int index] => _row[index] + _rowCost;

            public int Count => _row.Count;

            public IEnumerator<double> GetEnumerator()
            {
                return new PreferenceEnumerator(_rowCost, _row.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class PreferenceEnumerator : IEnumerator<double>
            {
                private readonly double _rowCost;
                private readonly IEnumerator<double> _rowEnum;

                public PreferenceEnumerator(double rowCost, IEnumerator<double> rowEnum)
                {
                    _rowCost = rowCost;
                    _rowEnum = rowEnum;
                }

                public double Current => _rowEnum.Current + _rowCost;

                object IEnumerator.Current => Current;

                public void Dispose()
                {
                    _rowEnum.Dispose();
                }

                public bool MoveNext()
                {
                    return _rowEnum.MoveNext();
                }

                public void Reset()
                {
                    _rowEnum.Reset();
                }
            }
        }
    }
}
