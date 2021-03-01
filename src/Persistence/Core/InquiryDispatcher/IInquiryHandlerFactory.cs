using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryHandler;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.InquiryDispatcher
{
    public interface IInquiryHandlerFactory
    {
        IInquiryHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IInquiry<TResponse>
            where TResponse : IInquiryResult;
    }
}