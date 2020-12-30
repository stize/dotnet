using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public class PagedValueQueryHandler<TQuery, TSource, TTarget, TResult> : QueryHandler<TQuery, TSource, TTarget, TResult>, IPagedValueQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : PagedQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : PagedQueryResult<TTarget>, new()
    {
        private int count;

        public PagedValueQueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }

        protected override async Task<IQueryable<TSource>> RunQueryAsync(CancellationToken cancellationToken = default)
        {
            var queryable = await base.RunQueryAsync(cancellationToken);
            this.count = await this.Provider.CountAsync(queryable, cancellationToken);
            var sorted = this.Sort(queryable);
            var paginated = this.Paginate(sorted);
            return paginated;
        }

        protected virtual IQueryable<TSource> Paginate(IQueryable<TSource> queryable)
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

        protected virtual IQueryable<TSource> Sort(IQueryable<TSource> queryable)
        {
            var effectiveSort = this.Query.Sorts.ToArray();
            return queryable.Sort(effectiveSort);
        }

        protected override async Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Provider.ToArrayAsync(queryable, cancellationToken);
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

    public class PagedValueQueryHandler<TQuery, TSource, TResult> : PagedValueQueryHandler<TQuery, TSource, TSource, TResult>, IPagedValueQueryHandler<TQuery, TSource, TResult>
        where TQuery : PagedQuery<TSource>
        where TSource : class
        where TResult : PagedQueryResult<TSource>, new()
    {
        public PagedValueQueryHandler(IMaterializer<TSource> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }
    }
}