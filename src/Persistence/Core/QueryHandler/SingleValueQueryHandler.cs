using System.Linq;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public abstract class SingleValueQueryHandler<TQuery, TSource, TTarget> : QueryHandler<TQuery, TSource, TTarget, ISingleQueryResult<TTarget>>, ISingleValueQueryHandler<TQuery, TSource, TTarget>
        where TQuery : ISingleValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
        protected SingleValueQueryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override ISingleQueryResult<TTarget> GenerateResult(IQueryable<TTarget> queryable)
        {
            var value = queryable.SingleOrDefault();
            return new SingleQueryResult<TTarget>(value);
        }
    }
}