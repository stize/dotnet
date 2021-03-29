using System.Collections.Generic;
using System.Linq;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Inquiry
{
    public abstract class Inquiry<TSource, TTarget, TResult> : IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IValueResult<TTarget>
    {

        public IQueryable<TSource> SourceQuery { get; set; }
        public IQueryableProvider Provider { get; set; } = DefaultQueryableProvider.Instance;

        public ISpecification<TSource> SourceSpecification { get; set; }
        public ISpecification<TTarget> TargetSpecification { get; set; }

        public IEnumerable<SortDescriptor> SourceSorts { get; set; }
        public IEnumerable<SortDescriptor> TargetSorts { get; set; }

    }

}