using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{

    public abstract class PagedValueQueryHandlerBase<TQuery, TSource, TTarget, TResult>
       : QueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : PagedValueQuery<TSource, TTarget>, IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : PagedQueryResult<TTarget>, new()
    {
        private int count;

        protected PagedValueQueryHandlerBase(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override async Task<IQueryable<TTarget>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken)
        {
            var queryable = await base.ExecuteQueryAsync(query, cancellationToken);

            this.count = await this.Query.Provider.CountAsync(queryable, cancellationToken);

            var paginated = this.Paginate(queryable);

            return paginated;
        }

        protected virtual IQueryable<TTarget> Paginate(IQueryable<TTarget> queryable)
        {
            if (this.Query.Skip.HasValue)
            {
                queryable = queryable.Skip(this.Query.Skip.Value);
            }

            if (this.Query.Take.HasValue)
            {
                queryable = queryable.Take(this.Query.Take.Value);
            }

            return queryable;
        }
        
        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Query.Provider.ToArrayAsync(queryable, cancellationToken);
            var result = new TResult
            {
                Result = values,
                Total = this.count,
                Take = this.Query.Take,
                Skip = this.Query.Skip
            };
            return result;
        }
    }

    public class PagedValueQueryHandler<TSource, TTarget>
        : PagedValueQueryHandlerBase<PagedValueQuery<TSource, TTarget>, TSource, TTarget, PagedQueryResult<TTarget>>
        where TSource : class
        where TTarget : class
    {
        public PagedValueQueryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }
    }

    public class PagedValueQueryHandler<TSource>
        : PagedValueQueryHandlerBase<PagedValueQuery<TSource>, TSource, TSource, PagedQueryResult<TSource>>
        where TSource : class
    {
        public PagedValueQueryHandler(IMaterializer<TSource> materializer) : base(materializer)
        {
        }
    }
}