using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Result;
using Stize.Mediator;
using Stize.Mediator.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace Stize.CQRS.EntityFrameworkCore.Internal
{
    internal class EntityCqrsRequestHandlerFactory : IRequestHandlerFactory
    {
        public IRequestHandler<TRequest, TResult> GetHandler<TRequest, TResult>(IServiceProvider provider, Type handlerType)
            where TRequest : IRequest<TResult>
            where TResult : IValueResult
        {
            var requestType = typeof(TRequest);
            var requestTypeParameters = requestType.GetTypeInfo().GenericTypeArguments;

            var requestHandlerType = requestTypeParameters.Length == 3 ? typeof(IEntityCqrsRequestHandler<,,>) :
                                     requestTypeParameters.Length == 4 ? typeof(IEntityCqrsRequestHandler<,,,>) :
                                     null;
            if (requestHandlerType == null) return null;

            var ttrqi = requestType
                            .GetInterfaces()
                            .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IEntityCqrsRequest<,,,>) || i.GetGenericTypeDefinition() == typeof(IEntityCqrsRequest<,,>)))
                            .FirstOrDefault();
            if (ttrqi == null) return null;

            var th = requestHandlerType.MakeGenericType(ttrqi.GenericTypeArguments);
            var providerHandlers = provider.GetServices(th);
            if (providerHandlers == null) return null;

            var providerHandler = providerHandlers.FirstOrDefault(x => x is IRequestHandler<TRequest, TResult>);
            if (providerHandler == null) return null;

            return (IRequestHandler<TRequest, TResult>)providerHandler;
        }
    }
}
