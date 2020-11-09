namespace Stize.Persistence.QueryResult
{
    public interface IPagedQueryResult<out T> : IMultipleQueryResult<T>
        where T : class
    {
        /// <summary>
        ///     Total results
        /// </summary>
        int Total { get; }

        /// <summary>
        ///     Take
        /// </summary>
        int? Take { get; }

        /// <summary>
        ///     Skip
        /// </summary>
        int? Skip { get; }
    }
}