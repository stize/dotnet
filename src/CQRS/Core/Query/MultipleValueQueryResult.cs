using System.Collections.Generic;

namespace Stize.CQRS.Query.Domain
{
    public class MultipleValueQueryResult<TValue> : IQueryResult
    {
        public IReadOnlyList<TValue> Value { get; }
    }

    public class PagedValueQueryResult<TValue> : MultipleValueQueryResult<TValue>
    {
        public int Total { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
    

}
