using Stize.DotNet.Result;
using Stize.Mediator;
using Stize.Persistence.Inquiry;

namespace Stize.Persistence.Internal
{
    internal interface IInquiryHandler<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {

    }

    internal interface IInquiryHandler<TInquiry, TSource, TTarget, TResult> : IInquiryHandler<TSource, TTarget>, IRequestHandler<TInquiry, TResult>
        where TInquiry : IInquiry<TSource, TTarget>, IRequest<TResult>
        where TSource : class
        where TTarget : class
        where TResult : IValueResult<TTarget>
    {        
    }
}