using System;
using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    public class MultipleQueryResult<T> : QueryResult<IEnumerable<T>>, IMultipleQueryResult<T> 
        where T : class
    {
        
    }
}