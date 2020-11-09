using System.Linq;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public class MultipleValueQueryHandler<TQuery, TSource, TTarget> : QueryHandler<TQuery, TSource, TTarget, IMultipleQueryResult<TTarget>>, IMultipleValueQueryHandler<TQuery, TSource, TTarget>
        where TQuery : IMultipleValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
        public MultipleValueQueryHandler(IMaterializer<TSource, TTarget> materializer) : base(materializer)
        {
        }

        protected override IMultipleQueryResult<TTarget> GenerateResult(IQueryable<TTarget> queryable)
        {
            var values = queryable.ToArray();
            return new MultipleQueryResult<TTarget>(values);
        }
    }
}