using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IAsyncQueryHandler<in TQuery, TEntity, TTarget, TResult>
        where TQuery : IQuery<TEntity>
        where TEntity : class
        where TTarget : class
        where TResult : IQueryResult
    {
        /// <summary>
        ///     Given a queryable and a queryableEvaluator realizes the query asynchronously
        /// </summary>
        /// <param name="query">TQuery</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Data resulting from the query</returns>
        Task<TResult> RunAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}