using Stize.Persistence.QueryResult;

namespace Stize.Persistence.Query
{
    /// <summary>
    /// Single Value Query interface. Inherits from IQuery
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TResult">QueryResult type</typeparam>
    public interface ISingleValueQuery<TEntity, TResult> : IQuery<TEntity, ISingleQueryResult<TResult>>
        where TEntity : class
        where TResult : class
    {

    }
}