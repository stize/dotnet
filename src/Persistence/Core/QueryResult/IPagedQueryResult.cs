namespace Stize.Persistence.QueryResult
{
    public interface IPagedQueryResult<T> : IMultipleQueryResult<T>
        where T : class
    {
        /// <summary>
        /// Total results
        /// </summary>
        int Total { get; set; }

        /// <summary>
        /// Take
        /// </summary>
        int? Take { get; set; }

        /// <summary>
        /// Skip
        /// </summary>
        int? Skip { get; set; }
    }
}