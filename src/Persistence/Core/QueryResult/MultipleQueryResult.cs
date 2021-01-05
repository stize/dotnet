using System;
using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    public class MultipleQueryResult<T> : QueryResult<T>
        where T : class
    {
        public IEnumerable<T> Result { get; set; }
    }
}