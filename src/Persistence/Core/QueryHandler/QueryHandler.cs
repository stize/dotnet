using System.Linq;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public abstract class QueryHandler<TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : IQuery<TSource>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult
    {
        protected QueryHandler(IMaterializer<TSource, TTarget> materializer)
        {
            this.Materializer = materializer;
        }

        protected IMaterializer<TSource, TTarget> Materializer { get; }

        protected TQuery Query { get; private set; }

        public virtual TResult Handle(TQuery query)
        {
            this.Query = query;
            var queryable = this.RunQuery();
            var materialized = this.Materialize(queryable);
            var result = this.GenerateResult(materialized);
            return result;
        }

        protected virtual IQueryable<TSource> RunQuery()
        {
            return queryable.Where(this.Query.Specification);
        }

        protected virtual IQueryable<TTarget> Materialize(IQueryable<TSource> queryable)
        {
            return this.Materializer.Materialize(queryable);
        }

        protected abstract TResult GenerateResult(IQueryable<TTarget> queryable);
    }
}