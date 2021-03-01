using System.Collections.Generic;
using System.Linq;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.Inquiry
{
    public abstract class Inquiry<TResult> : IInquiry<TResult>
        where TResult : IInquiryResult
    {
        
    }

    public abstract class Inquiry<TSource, TTarget, TResult> : Inquiry<TResult>, IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IInquiryResult<TTarget>
    {

        public IQueryable<TSource> SourceQuery { get; set; }
        public IQueryableProvider Provider { get; set; } = DefaultQueryableProvider.Instance;

        public ISpecification<TSource> SourceSpecification { get; set; }
        public ISpecification<TTarget> TargetSpecification { get; set; }

        public IEnumerable<SortDescriptor> SourceSorts { get; set; }
        public IEnumerable<SortDescriptor> TargetSorts { get; set; }

    }

}