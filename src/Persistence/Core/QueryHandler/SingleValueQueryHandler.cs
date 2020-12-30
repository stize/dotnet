using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public class SingleValueQueryHandler<TQuery, TSource, TTarget, TResult> : QueryHandler<TQuery, TSource, TTarget, TResult>, ISingleValueQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : Query<TSource>
        where TSource : class
        where TTarget : class
        where TResult : SingleQueryResult<TTarget>, new()
    {
        public SingleValueQueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }

        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var value = await this.Provider.SingleOrDefaultAsync(queryable, cancellationToken);
            var result = new TResult { Result = value };
            return result;
        }
    }

    public class SingleValueQueryHandler<TQuery, TSource, TResult> : SingleValueQueryHandler<TQuery, TSource, TSource, TResult>, ISingleValueQueryHandler<TQuery, TSource, TResult>
        where TQuery : Query<TSource>
        where TSource : class
        where TResult : SingleQueryResult<TSource>, new()
    {
        public SingleValueQueryHandler(IMaterializer<TSource> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }
    }
}