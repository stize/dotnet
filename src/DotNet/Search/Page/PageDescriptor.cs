using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Sort;

namespace Stize.DotNet.Search.Page
{
    /// <summary>
    /// PageDescriptor Class implementation
    /// </summary>
    public class PageDescriptor : IPageDescriptor
    {
        /// <summary>
        /// Creates a new instance of  <see cref="PageDescriptor" />
        /// </summary>
        public PageDescriptor()
        {
            this.Filters = new Collection<FilterDescriptor>();
            this.Sorts = new Collection<SortDescriptor>();
        }

        /// <summary>
        /// Creates a new instance of  <see cref="PageDescriptor" />
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="sorts"></param>
        public PageDescriptor(ICollection<FilterDescriptor> filters, ICollection<SortDescriptor> sorts)
        {
            this.Filters = filters ?? new Collection<FilterDescriptor>();
            this.Sorts = sorts ?? new Collection<SortDescriptor>();
        }

        /// <summary>
        /// Number of elements to skip in a section
        /// </summary>
        public int? Skip { get; set; }
        /// <summary>
        /// Number of elements to select
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// Collection of FiltersDescriptors
        /// </summary>
        public ICollection<FilterDescriptor> Filters { get; set; }
        /// <summary>
        /// Collection of SortDescriptors
        /// </summary>
        public ICollection<SortDescriptor> Sorts { get; set; }
    }
}