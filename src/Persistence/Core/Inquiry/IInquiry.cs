using System.Collections.Generic;
using System.Linq;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Sort;
using Stize.DotNet.Specification;
using Stize.Mediator;

namespace Stize.Persistence.Inquiry
{
    public interface IInquiry<TResult> : IRequest<TResult>
        where TResult : IValueResult
    {

    }

    public interface IInquiry<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {

        public IQueryableProvider Provider { get; set; }
        public IQueryable<TSource> SourceQuery { get; set; }

        ISpecification<TSource> SourceSpecification { get; }
        ISpecification<TTarget> TargetSpecification { get; }

        public IEnumerable<SortDescriptor> SourceSorts { get; }
        public IEnumerable<SortDescriptor> TargetSorts { get; }
    }

    public interface IInquiry<TSource, TTarget, TResult> : IInquiry<TSource, TTarget>, IInquiry<TResult>
        where TSource : class
        where TTarget : class
        where TResult : IValueResult
    { 
    }

}