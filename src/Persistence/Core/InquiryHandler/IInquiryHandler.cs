using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryResult;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Persistence.InquiryHandler
{

    public interface IInquiryHandler<TRequest, TResponse>
        where TRequest : IInquiry<TResponse>
        where TResponse : IInquiryResult
    {
        Task<TResponse> HandleAsync(TRequest inquiry, CancellationToken cancellationToken = default);
    }

    public interface IInquiryHandler<TInquiry, TSource, TTarget, TResult> : IInquiryHandler<TInquiry, TResult>
        where TInquiry : IInquiry<TSource, TTarget, TResult>
        where TSource : class
        where TTarget : class
        where TResult : IInquiryResult<TTarget>
    {        
    }
}