using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface ISingleValueQueryHandler<in TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : class, ISingleQueryResult<TTarget>, new()
    {
    }

    public interface ISingleValueQueryHandler<in TQuery, TSource, TResult> : ISingleValueQueryHandler<TQuery, TSource, TSource, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TResult : class, ISingleQueryResult<TSource>, new()
    {

    }
}