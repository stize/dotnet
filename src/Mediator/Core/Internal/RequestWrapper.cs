using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Mediator.Internal
{
    internal abstract class RequestWrapper<TResult>
        where TResult : IValueResult
    {
        public abstract Task<TResult> HandleAsync(IServiceProvider provider, IRequest<TResult> request, CancellationToken cancellationToken = default);
    }

    internal class RequestWrapper<TRequest, TResult> : RequestWrapper<TResult>
        where TRequest : IRequest<TResult>
        where TResult : IValueResult
    {
        public override async Task<TResult> HandleAsync(IServiceProvider provider, IRequest<TResult> request, CancellationToken cancellationToken = default)
        {
            var requestType = typeof(TRequest);
            var resultType = typeof(TResult);

            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, resultType);
            var handler = this.GetHandler(provider, handlerType);
            var wrapper = Activator.CreateInstance<RequestHandlerWrapper<TRequest, TResult>>();

            var result = await wrapper.HandleAsync(handler, (TRequest)request, cancellationToken);
            return result;
        }

        private IRequestHandler<TRequest, TResult> GetHandler(IServiceProvider provider, Type handlerType)
        {
            var providerHandler = provider.GetService(handlerType);
            if (providerHandler != null)
            {
                return (IRequestHandler<TRequest, TResult>)providerHandler;
            }

            var factories = provider.GetServices<IRequestHandlerFactory>();
            foreach (var factory in factories)
            {
                var factoryHandler = factory.GetHandler<TRequest, TResult>(provider, handlerType);
                if(factoryHandler != null)
                {
                    return factoryHandler;
                }
            }

            throw new Exception($"No handler found for {typeof(TRequest).FullName}");
        }
    }

    public interface IRequestHandlerFactory
    {
        IRequestHandler<TRequest, TResult> GetHandler<TRequest, TResult>(IServiceProvider provider, Type handlerType)
            where TRequest : IRequest<TResult>
            where TResult : IValueResult;
    }
}
