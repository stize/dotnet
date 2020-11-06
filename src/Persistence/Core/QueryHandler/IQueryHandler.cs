using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : IQuery<TSource, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {
        /// <summary>
        /// Given a queryable and a queryableEvaluator realizes the query
        /// </summary>
        /// <param name="query">TQuery</param>
        /// <returns>Data resulting from the query</returns>
        TResult Run(TQuery query);

    }

    public abstract class QueryHandler<TQuery, TSource, TTarget, TResult> : IQueryHandler<TQuery, TSource, TTarget, TResult>
        where TQuery : IQuery<TSource, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IQueryResult<TTarget>
    {
        protected IEntityMaterializer<TSource, TTarget> Materializer { get; }

        protected TQuery Query { get; private set; }

        protected QueryHandler(IEntityMaterializer<TSource, TTarget> materializer)
        {
            this.Materializer = materializer;
        }

        public virtual TResult Run(TQuery query)
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
