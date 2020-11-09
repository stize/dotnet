using Stize.Persistence.Query;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.QueryHandler
{
    public interface IMultipleValueQueryHandler<in TQuery, TSource, TTarget> : IQueryHandler<TQuery, TSource, TTarget, IMultipleQueryResult<TTarget>>
        where TQuery : IMultipleValueQuery<TSource>
        where TSource : class
        where TTarget : class
    {
    }

    public interface IMultipleValueQueryHandler<in TQuery, TSource> : IMultipleValueQueryHandler<TQuery, TSource, TSource>
        where TQuery : IMultipleValueQuery<TSource>
        where TSource : class
    {

    }
}