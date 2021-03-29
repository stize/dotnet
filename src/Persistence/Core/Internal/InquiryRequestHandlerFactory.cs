using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Result;
using Stize.Mediator;
using Stize.Mediator.Internal;
using Stize.Persistence.Inquiry;
using System;
using System.Linq;

namespace Stize.Persistence.Internal
{
    internal class InquiryHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler<TRequest, TResult> GetHandler<TRequest, TResult>(IServiceProvider provider, Type handlerType)
            where TRequest : IRequest<TResult>
            where TResult : IValueResult
        {
            var ti = typeof(IInquiryHandler<,>);
            var ttrqi = typeof(TRequest).GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInquiry<,>)).FirstOrDefault();
            if (ttrqi == null) return null;

            var th = ti.MakeGenericType(ttrqi.GenericTypeArguments);           
            var providerHandlers = provider.GetServices(th);
            if (providerHandlers == null) return null;

            var providerHandler = providerHandlers.FirstOrDefault(x => x is IRequestHandler<TRequest, TResult>);
            if (providerHandler == null) return null;

            return (IRequestHandler<TRequest, TResult>)providerHandler;
        }
    }
}
