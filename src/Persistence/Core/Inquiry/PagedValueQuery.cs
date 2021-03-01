using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public class PagedValueQuery<TSource, TTarget> : Query<TSource, TTarget, PagedQueryResult<TTarget>>
        where TSource : class
        where TTarget : class

    {
        public int? Take { get; set; }

        public int? Skip { get; set; }

    }

    public class PagedValueQuery<TSource> : PagedValueQuery<TSource, TSource>
        where TSource : class
    {
    }
}