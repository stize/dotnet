
using Stize.DotNet.Specification;

namespace System.Linq
{
    public static class QueryableSpecificationExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> query, ISpecification<T> specification)
        {
            return query.Where(specification.Predicate);
        }
    }
}
