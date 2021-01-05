using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public abstract class MultipleValueQueryHandlerBase<TQuery, TSource, TTarget, TResult> 
        : QueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery:  MultipleValueQuery<TSource, TTarget>, IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : MultipleQueryResult<TTarget>, new()
    {
        protected MultipleValueQueryHandlerBase(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }
        
        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Query.Provider.ToArrayAsync(queryable, cancellationToken);
            var result = new TResult { Result = values };
            return result;
        }
    }

    public class MultipleValueQueryHandler<TSource, TTarget> 
        : MultipleValueQueryHandlerBase<MultipleValueQuery<TSource, TTarget>, TSource, TTarget, MultipleQueryResult<TTarget>>
        where TSource : class
        where TTarget : class
    {
        public MultipleValueQueryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

    }

    public class MultipleValueQueryHandler<TSource> 
        : MultipleValueQueryHandlerBase<MultipleValueQuery<TSource>, TSource, TSource, MultipleQueryResult<TSource>>
        where TSource : class
    {
        public MultipleValueQueryHandler(IMaterializer<TSource> materializer) : base(materializer)
        {
        }
    }
}