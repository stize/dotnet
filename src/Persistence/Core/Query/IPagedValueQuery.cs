using System.Collections.Generic;
using Stize.DotNet.Search.Sort;

namespace Stize.Persistence.Query
{
    public interface IPagedValueQuery<T> : IQuery<T>
        where T : class
    {
        int? Take { get; }
        int? Skip { get; }

        public IEnumerable<SortDescriptor> Sorts { get; }
    }
}