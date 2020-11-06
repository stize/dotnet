namespace Stize.Persistence.QueryResult
{
    /// <summary>
    /// Single query result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleQueryResult<T> : IQueryResult<T> 
        where T : class
    {
        /// <summary>
        /// Data resulting from the query
        /// </summary>
        T Result { get; }
    }
}