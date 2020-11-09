using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public class PagedValueQueryHandler<TQuery, TSource, TTarget> : QueryHandler<TQuery, TSource, TTarget, IPagedQueryResult<TTarget>>, IPagedValueQueryHandler<TQuery, TSource, TTarget>
        where TQuery : IPagedValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
        private int count;

        public PagedValueQueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider) : base(materializer,provider)
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
            if (this.Query.Take.HasValue)
            {
                queryable = queryable.Take(this.Query.Take.Value);
            }

            if (this.Query.Skip.HasValue)
            {
                queryable = queryable.Skip(this.Query.Skip.Value);
            }

            return queryable;
        }

        protected virtual IQueryable<TSource> Sort(IQueryable<TSource> queryable)
        {
            var effectiveSort = this.Query.Sorts.ToArray();
            return queryable.Sort(effectiveSort);
        }
        
        protected override async Task<IPagedQueryResult<TTarget>> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Provider.ToArrayAsync(queryable, cancellationToken);
            var pagedResult = new PagedQueryResult<TTarget>(values, this.count, this.Query.Take, this.Query.Skip);
            return pagedResult;
        }
    }

    public class PagedValueQueryHandler<TQuery, TSource> : PagedValueQueryHandler<TQuery, TSource, TSource>, IPagedValueQueryHandler<TQuery, TSource>
        where TQuery : IPagedValueQuery<TSource>
        where TSource : class
    {
        public PagedValueQueryHandler(IMaterializer<TSource> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }
    }
}