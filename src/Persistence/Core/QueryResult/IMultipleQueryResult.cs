using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    /// <summary>
    ///     Multiple query result interface
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IMultipleQueryResult<out T> : IQueryResult
        where T : class
    {
        IEnumerable<T> Result { get; }
    }
}