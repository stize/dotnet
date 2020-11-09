using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IPagedValueQueryHandler<in TQuery, TSource, TTarget> : IQueryHandler<TQuery, TSource, TTarget, IPagedQueryResult<TTarget>>
        where TQuery : IPagedValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
    }
}