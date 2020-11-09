using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public MultipleValueQueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider) : base(materializer,provider)
        {
        }
        
        protected override async Task<IMultipleQueryResult<TTarget>> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var values = await this.Provider.ToArrayAsync(queryable, cancellationToken);
            return new MultipleQueryResult<TTarget>(values);
        }
    }
}