using System;
using System.Collections.Generic;

namespace GameSolver.NET.Extensions
{
    public static class IEnumerableExt
    {
        public static T MinBy<T, TC>(this IEnumerable<T> sequence, Func<T, TC> keySelector)
        {
            var first = true;
            var result = default(T);
            var minKey = default(TC);
            IComparer<TC> comparer = Comparer<TC>.Default; //or you can pass this in as a parameter

            foreach (var item in sequence)
            {
                if (first)
                {
                    result = item;
                    minKey = keySelector.Invoke(item);
                    first = false;
                    continue;
                }

                var key = keySelector.Invoke(item);
                if (comparer.Compare(key, minKey) < 0)
                {
                    result = item;
                    minKey = key;
                }
            }

            return result;
        }
    }
}
