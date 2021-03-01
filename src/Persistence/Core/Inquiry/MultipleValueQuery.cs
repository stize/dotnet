using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public class MultipleValueQuery<TSource, TTarget> : Query<TSource, TTarget, MultipleQueryResult<TTarget>>
        where TSource : class 
        where TTarget : class

    {
    }

    public class MultipleValueQuery<TSource> : MultipleValueQuery<TSource, TSource>
        where TSource : class 
    {
    }
}