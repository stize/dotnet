using System.Collections.Generic;
using System.Linq;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.Inquiry
{
    public interface IInquiry<TResult>
        where TResult : IInquiryResult
    {
       
    }

    public interface IInquiry<TSource, TTarget, TResult> : IInquiry<TResult>
        where TSource : class
        where TTarget : class
        where TResult : IInquiryResult<TTarget>
    {
        
        public IQueryableProvider Provider { get; set; }
        public IQueryable<TSource> SourceQuery { get; set; }

        ISpecification<TSource> SourceSpecification { get; }
        ISpecification<TTarget> TargetSpecification { get; }

        public IEnumerable<SortDescriptor> SourceSorts { get; }
        public IEnumerable<SortDescriptor> TargetSorts { get; }

        
    }



}