using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public class MultipleValueQueryHandler<TQuery, TSource, TTarget, TResult> : QueryHandler<TQuery, TSource, TTarget, TResult>, IMultipleValueQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : Query<TSource>
        where TSource : class
        where TTarget : class
        where TResult : MultipleQueryResult<TTarget>, new()
    {
        public MultipleValueQueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider) : base(materializer,provider)
        {
        }
        
        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Provider.ToArrayAsync(queryable, cancellationToken);
            var result = new TResult { Result = values };
            return result;
        }
    }

    public class MultipleValueQueryHandler<TQuery, TSource, TResult> : MultipleValueQueryHandler<TQuery, TSource, TSource, TResult>, IMultipleValueQueryHandler<TQuery, TSource, TResult>
        where TQuery : Query<TSource>
        where TSource : class
        where TResult : MultipleQueryResult<TSource>, new()
    {
        public MultipleValueQueryHandler(IMaterializer<TSource> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }
    }
}