using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IQueryHandler<in TQuery, TSource, TTarget, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}