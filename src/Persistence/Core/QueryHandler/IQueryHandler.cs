using Stize.Persistence.Mediator;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{

    public interface IQueryHandler<TQuery, TSource, TTarget, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {
    }
}