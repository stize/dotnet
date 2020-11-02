using System.Reflection;
using Stize.DotNet.Search.Sort;

namespace System.Linq
{
    public static class QueryableSortExtensions
    {
        public static MethodInfo GetOrderMethod(this SortDirection direction)
        {
            return direction == SortDirection.Ascending ? QueryableExtensions.OrderByMethod : QueryableExtensions.OrderByDescendingMethod;
        }

        public static MethodInfo GetThenMethod(this SortDirection direction)
        {
            return direction == SortDirection.Ascending ? QueryableExtensions.ThenByMethod : QueryableExtensions.ThenByDescendingMethod;
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, SortDescriptor[] effectiveSort)
        {
            if (effectiveSort == null)
            {
                return queryable;
            }

            var first = effectiveSort.FirstOrDefault();
            if (first != null)
            {
                var method = GetOrderMethod(first.Direction);
                queryable = queryable.OrderByPropertyName(method, first.Member);
            }

            foreach (var sort in effectiveSort.Skip(1))
            {
                var method = GetThenMethod(sort.Direction);
                queryable = queryable.OrderByPropertyName(method, sort.Member);
            }

            return queryable;
        }
    }
}