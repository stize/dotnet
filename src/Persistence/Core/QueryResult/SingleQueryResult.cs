using System;

namespace Stize.Persistence.QueryResult
{
    public class SingleQueryResult<T> : QueryResult<T>, ISingleQueryResult<T> 
        where T : class
    {
    }
}