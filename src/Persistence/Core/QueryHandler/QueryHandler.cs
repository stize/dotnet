using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public abstract class QueryHandler<TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : IQuery<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {
        protected QueryHandler(IMaterializer<TSource, TTarget> materializer)
        {
            this.Materializer = materializer;
        }

        protected IMaterializer<TSource, TTarget> Materializer { get; }

        protected TQuery Query { get; set; }

        public virtual async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            var queryable = await this.ExecuteQueryAsync(query, cancellationToken);

            var result = await this.GenerateResultAsync(queryable, cancellationToken);
            return result;
        }

        protected virtual async Task<IQueryable<TTarget>> ExecuteQueryAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            this.Query = query;
            var queryable = await this.GetSourceQueryAsync(cancellationToken);

            var sorted = await this.SortAsync(queryable, cancellationToken);
            var filtered = await this.FilterAsync(sorted, cancellationToken);

            var materialized = await this.MaterializeAsync(filtered, cancellationToken);

            var sortedMaterialized = await this.SortAsync(materialized, cancellationToken);
            var filteredMaterialized = await this.FilterAsync(sortedMaterialized, cancellationToken);
            return filteredMaterialized;
        }

        protected virtual Task<IQueryable<TSource>> GetSourceQueryAsync(CancellationToken cancellationToken = default)
        {
            var query = this.Query.SourceQuery;
            return Task.FromResult(query);
        }

        protected virtual Task<IQueryable<TSource>> FilterAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Query.SourceSpecification != null)
            {
                queryable = queryable.Where(this.Query.SourceSpecification);
            }
                
            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TTarget>> FilterAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Query.TargetSpecification != null)
            {
                queryable = queryable.Where(this.Query.TargetSpecification);
            }
                
            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TSource>> SortAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Query.SourceSorts != null)
            {
                var effectiveSort = this.Query.SourceSorts.ToArray();
                queryable = queryable.Sort(effectiveSort);
            }

            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TTarget>> SortAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            if (this.Query.TargetSorts != null)
            {
                var effectiveSort = this.Query.TargetSorts.ToArray();
                queryable = queryable.Sort(effectiveSort);
            }

            return Task.FromResult(queryable);
        }

        protected virtual Task<IQueryable<TTarget>> MaterializeAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            var materialized = this.Materializer.Materialize(queryable);
            return Task.FromResult(materialized);
        }

        protected abstract Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default);

    }


}