using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IQueryHandler<in TQuery, TSource, TTarget, out TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult
    {
        TResult Handle(TQuery query);
    }
}