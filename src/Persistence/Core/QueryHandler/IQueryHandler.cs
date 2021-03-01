using Stize.Persistence.Query;
using Stize.Persistence.QueryDispatcher;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{

    public interface IQueryHandler<TQuery, TSource, TTarget, TResult> : IQueryRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {
    }
}