using Stize.DotNet.Result;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryHandler;

namespace Stize.Persistence.InquiryDispatcher
{
    public interface IInquiryHandlerFactory
    {
        IInquiryHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IInquiry<TResponse>
            where TResponse : IValueResult;
    }
}