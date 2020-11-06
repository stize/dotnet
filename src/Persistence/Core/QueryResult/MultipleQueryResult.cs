using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    public class MultipleQueryResult<T> : IMultipleQueryResult<T> where T : class
    {
        public MultipleQueryResult(IEnumerable<T> result)
        {
            this.Result = result;
        }

        public IEnumerable<T> Result { get; }
    }
}