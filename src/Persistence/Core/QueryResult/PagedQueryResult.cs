using System;
using System.Collections.Generic;

namespace Stize.Persistence.QueryResult
{
    public class PagedQueryResult<T> : MultipleQueryResult<T>, IPagedQueryResult<T> 
        where T : class
    {

        public int Total { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
}