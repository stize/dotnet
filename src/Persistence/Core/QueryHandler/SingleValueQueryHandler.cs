using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Materializer;
using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public class SingleValueQueryHandler<TQuery, TSource, TTarget> : QueryHandler<TQuery, TSource, TTarget, ISingleQueryResult<TTarget>>, ISingleValueQueryHandler<TQuery, TSource, TTarget>
        where TQuery : ISingleValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
        protected SingleValueQueryHandler(IMaterializer<TSource, TTarget> materializer, IQueryableProvider provider) : base(materializer, provider)
        {
        }
        
        protected override async Task<ISingleQueryResult<TTarget>> GenerateResultAsync(IQueryable<TTarget> queryable, CancellationToken cancellationToken = default)
        {
            var value = await this.Provider.SingleOrDefaultAsync(queryable, cancellationToken);
            return new SingleQueryResult<TTarget>(value);
        }
    }
}