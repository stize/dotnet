using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface ISingleValueQueryHandler<in TQuery, TSource, TTarget> : IQueryHandler<TQuery, TSource, TTarget, ISingleQueryResult<TTarget>>
        where TQuery : ISingleValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
    }
}