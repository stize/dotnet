using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IPagedValueQueryHandler<in TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IPagedQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : class, IPagedQueryResult<TTarget>, new()
    {
    }

    public interface IPagedValueQueryHandler<in TQuery, TSource, TResult> : IPagedValueQueryHandler<TQuery, TSource, TSource, TResult>
        where TQuery : IPagedQuery<TSource>
        where TSource : class
        where TResult : class, IPagedQueryResult<TSource>, new()
    {

    }
}