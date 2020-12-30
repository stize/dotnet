using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public abstract class QueryHandler<TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : class, IQueryResult, new()
    {
        protected QueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider)
        {
            this.Provider = provider;
            this.Materializer = materializer;
        }

        protected IMaterializer<TSource, TTarget> Materializer { get; }

        protected IQueryableProvider Provider { get; }

        protected TQuery Query { get; private set; }

        public virtual async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
        {
            this.Query = query;
            var queryable = await this.RunQueryAsync(cancellationToken);
            var materialized = await this.MaterializeAsync(queryable, cancellationToken);
            var result = await this.GenerateResultAsync(materialized, cancellationToken);
            return result;
        }

        protected virtual Task<IQueryable<TSource>> RunQueryAsync(CancellationToken cancellationToken = default)
        {
            var query = this.Provider.GetQueryable<TSource>().Where(this.Query.Specification);
            return Task.FromResult(query);
        }

        protected virtual Task<IQueryable<TTarget>> MaterializeAsync(IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
        {
            var materialized = this.Materializer.Materialize(queryable);
            return Task.FromResult(materialized);
        }

        protected abstract Task<TResult> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default);

    }


}