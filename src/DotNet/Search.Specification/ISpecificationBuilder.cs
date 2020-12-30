using System.Collections.Generic;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Specification;

namespace Stize.DotNet.Search.Specification
{
    public interface ISpecificationBuilder
    {
        ISpecification<T> Create<T>(IEnumerable<FilterDescriptor> filters);
    }
}