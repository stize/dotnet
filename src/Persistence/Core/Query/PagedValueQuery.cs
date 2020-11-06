using System.Collections.Generic;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public class PagedValueQuery<TEntity, TResult> : MultipleValueQuery<TEntity, TResult>, IPagedValueQuery<TEntity, TResult> 
        where TEntity : class 
        where TResult : class
    {
        public PagedValueQuery(ISpecification<TEntity> specification, int? take = null, int? skip = null) : base(specification)
        {
            this.Take = take;
            this.Skip = skip;
        }

        public int? Take { get; }
        public int? Skip { get; }

        public IEnumerable<SortDescriptor> Sorts { get; set; } = new SortDescriptor[] { };
    }
}