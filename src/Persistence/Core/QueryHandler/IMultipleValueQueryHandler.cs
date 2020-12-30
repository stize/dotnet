using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IMultipleValueQueryHandler<in TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : class, IMultipleQueryResult<TTarget>, new()
    {
    }

    public interface IMultipleValueQueryHandler<in TQuery, TSource, TResult> : IMultipleValueQueryHandler<TQuery, TSource, TSource, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TResult : class, IMultipleQueryResult<TSource>, new()
    {

    }
}