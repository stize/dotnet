using System.Linq;
using Stize.DotNet.Specification;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    public class SingleValueQuery<TSource, TTarget> : Query<TSource, TTarget, SingleQueryResult<TTarget>>
        where TSource : class 
        where TTarget : class

    {
    }

    public class SingleValueQuery<TSource> : SingleValueQuery<TSource, TSource>
        where TSource : class 
    {
    }
}