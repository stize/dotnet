namespace Stize.Persistence.QueryResult
{
    /// <summary>
    /// Paged query result interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IPagedQueryResult<T> : IMultipleQueryResult<T>
        where T : class
    {
        /// <summary>
        /// Total results
        /// </summary>
        int Total { get; }

        /// <summary>
        /// Take
        /// </summary>
        int? Take { get; }

        /// <summary>
        /// Skip
        /// </summary>
        int? Skip { get; }
    }
}