using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    public class PagedQueryResult<T> : MultipleQueryResult<T>, IPagedQueryResult<T> 
        where T : class
    {
        public PagedQueryResult(IEnumerable<T> result, int total, int? take = null, int? skip = null) : base(result)
        {
            this.Total = total;
            this.Skip = skip;
            this.Take = take;
        }

        public int Total { get; }
        public int? Take { get; }
        public int? Skip { get; }
    }
}