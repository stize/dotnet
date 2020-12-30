using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    public interface IMultipleQueryResult<T> : IQueryResult<IEnumerable<T>>
        where T : class
    {
        
    }
}