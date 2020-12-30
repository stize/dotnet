using System.Collections.Generic;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public class PagedQuery<T> : Query<T>, IPagedQuery<T>
        where T : class
    {
        public PagedQuery(ISpecification<T> specification, int? take = null, int? skip = null) : base(specification)
        {
            this.Take = take;
            this.Skip = skip;
        }

        public int? Take { get; }
        public int? Skip { get; }

        public IEnumerable<SortDescriptor> Sorts { get; set; } = new SortDescriptor[] { };
    }
}