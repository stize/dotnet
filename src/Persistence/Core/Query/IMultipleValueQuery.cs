using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    /// <summary>
    /// Multiple Value Query Interface. Inherits from IQuery
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TResult">QueryResult type</typeparam>
    public interface IMultipleValueQuery<TEntity, TResult> : IQuery<TEntity, IMultipleQueryResult<TResult>>
        where TEntity : class
        where TResult : class
    {

    }
}