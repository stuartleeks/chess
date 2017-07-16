using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chess.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
            where T : struct
        {
            return items
                        .Where(NotNull)
                        .Select(item => item.Value);
        }
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> items)
            where T : class
        {
            return items
                        .Where(NotNull);
        }
        private static bool NotNull<T>(T item)
        {
            return item != null;
        }

        public static IEnumerable<Tuple<T,T>> Pair<T>(this IEnumerable<T> items)
            where T : class
        {
            T item1 = null;

            bool odd = true;
            foreach (var item in items)
            {
                if (odd)
                {
                    item1 = item;
                }
                else
                {
                    // got a pair
                    yield return new Tuple<T, T>(item1, item);
                    item1 = null;
                }
                odd = !odd;
            }
            if (!odd)
            {
                yield return new Tuple<T, T>(item1, null);
            }
        }
    }
}
