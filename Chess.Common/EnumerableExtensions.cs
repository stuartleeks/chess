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
        private static bool NotNull<T>(T item)
        {
            return item != null;
        }
    }
}
