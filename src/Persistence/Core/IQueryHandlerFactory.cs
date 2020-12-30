using Stize.Persistence.Query;
using Stize.Persistence.QueryHandler;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence
{
    public interface IQueryHandlerFactory
    {
        IQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>()
            where TQuery : IQuery
            where TResult : class, IQueryResult, new();
    }
}