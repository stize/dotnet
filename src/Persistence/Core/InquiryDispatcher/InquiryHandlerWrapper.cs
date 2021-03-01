using System;
using System.Threading;
using System.Threading.Tasks;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryHandler;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.InquiryDispatcher
{
    internal abstract class InquiryHandlerWrapper<TResult>
        where TResult : IInquiryResult
    {
        public abstract Task<TResult> HandleAsync(IInquiry<TResult> inquiry, IInquiryHandlerFactory handlerFactory, CancellationToken cancellationToken = default);
    }

    internal class InquiryHandlerWrapper<TInquiry, TResult> : InquiryHandlerWrapper<TResult>
        where TInquiry : class, IInquiry<TResult>
        where TResult : IInquiryResult
    {

        public override async Task<TResult> HandleAsync(IInquiry<TResult> inquiry, IInquiryHandlerFactory handlerFactory, CancellationToken cancellationToken = default)
        {
            var q = inquiry as TInquiry;

            var handler = GetHandler(handlerFactory);
            var result = await handler.HandleAsync(q, cancellationToken);
            return result;
        }

        public IInquiryHandler<TInquiry, TResult> GetHandler(IInquiryHandlerFactory handlerFactory)
        {
            var handler = handlerFactory.GetHandler<TInquiry, TResult>();
            if (handler == null)
            {
                throw new ArgumentException($"Cant find a handler for inquiry {typeof(TInquiry).FullName}");
            }

            return handler;
        }
    }
}