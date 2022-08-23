using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShadowrunTracker.Utils
{
    public static class EnumerableExtensions
    {
        public static void Sort<TSource>(this ObservableCollection<TSource> source, IComparer<TSource> comparer)
        {
            var list = source.ToList();
            list.Sort(comparer);
            source.Clear();
            foreach (var item in list)
            {
                source.Add(item);
            }
        }

        public static void SortBy<TSource>(this ObservableCollection<TSource> source, Func<TSource, IComparable> keySelector)
        {
            var list = source.OrderBy(keySelector);
            source.Clear();
            foreach (var item in list)
            {
                source.Add(item);
            }
        }

        public static IEnumerable<T> TakeUntilIncluding<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            foreach (T el in list)
            {
                yield return el;
                if (predicate(el))
                    yield break;
            }
        }
    }
}
