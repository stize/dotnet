using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public abstract class SingleValueQueryHandlerBase<TQuery, TSource, TTarget, TResult>
        : QueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery:  SingleValueQuery<TSource, TTarget>, IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : SingleQueryResult<TTarget>, new()
    {
        protected SingleValueQueryHandlerBase(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var value = await this.Query.Provider.SingleOrDefaultAsync(queryable, cancellationToken);
            var result = new TResult() { Result = value };
            return result;
        }
    }

    public class SingleValueQueryHandler<TSource, TTarget> 
        : SingleValueQueryHandlerBase<SingleValueQuery<TSource, TTarget>, TSource, TTarget, SingleQueryResult<TTarget>>
        where TSource : class
        where TTarget : class
    {
        public SingleValueQueryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }
        
        
    }

    public class SingleValueQueryHandler<TSource> 
        : SingleValueQueryHandlerBase<SingleValueQuery<TSource>, TSource, TSource, SingleQueryResult<TSource>>
        where TSource : class
    {
        public SingleValueQueryHandler(IMaterializer<TSource> materializer) : base(materializer)
        {
        }
        
    }
}