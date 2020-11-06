using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    /// <summary>
    /// Multiple query result interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>

    public interface IMultipleQueryResult<T> : IQueryResult<T> where T : class
    {
        /// <summary>
        /// Data resulting form the query in <typeparamref name="T"/>
        /// </summary>
        IEnumerable<T> Result { get; }
    }
}