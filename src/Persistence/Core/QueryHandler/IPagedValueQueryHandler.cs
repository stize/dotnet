using System.Linq;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IPagedValueQueryHandler<TQuery, TSource, TTarget> : IQueryHandler<TQuery, TSource, TTarget, IPagedQueryResult<TTarget>>
        where TQuery : IPagedValueQuery<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
    }

    public class PagedValueQueryHandler<TQuery, TSource, TTarget> : QueryHandler<TQuery, TSource, TTarget, IPagedQueryResult<TTarget>>, IPagedValueQueryHandler<TQuery, TSource, TTarget>
        where TQuery : IPagedValueQuery<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        private int count = 0;

        public PagedValueQueryHandler(IEntityMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override IQueryable<TSource> RunQuery()
        {
            var queryable = base.RunQuery();
            this.count = queryable.Count();
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

        protected override IPagedQueryResult<TTarget> GenerateResult(IQueryable<TTarget> queryable)
        {
            var values = queryable.ToArray();
            return new PagedQueryResult<TTarget>(values, this.count, this.Query.Take, this.Query.Skip);
        }
    }
}