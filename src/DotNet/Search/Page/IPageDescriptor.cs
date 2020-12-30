using System.Collections.Generic;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Sort;

namespace Stize.DotNet.Search.Page
{
    /// <summary>
    /// PageDescriptor interface
    /// </summary>
    public interface IPageDescriptor : IPageSize
    {
        /// <summary>
        /// Collection of filters
        /// </summary>
        ICollection<FilterDescriptor> Filters { get; set;}
        /// <summary>
        /// Collection of sorts
        /// </summary>
        ICollection<SortDescriptor> Sorts { get; set;}
    }
}