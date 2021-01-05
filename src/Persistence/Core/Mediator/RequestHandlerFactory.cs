using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Stize.Persistence.Mediator
{
    public class RequestHandlerFactory : IRequestHandlerFactory
    {
        private readonly ILogger<RequestHandlerFactory> logger;
        private readonly RequestHandlerFactoryOptions options;
        private readonly IServiceProvider provider;

        public RequestHandlerFactory(ILogger<RequestHandlerFactory> logger, IOptions<RequestHandlerFactoryOptions> options, IServiceProvider provider)
        {
            this.logger = logger;
            this.options = options.Value;
            this.provider = provider;
        }

        public IRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IRequest<TResponse>
        {
            var targetHandler = typeof(IRequestHandler<TRequest, TResponse>);
            var effectiveHandlerTypes = new List<Type>();

            foreach (var handler in this.options.QueryHandlers)
            {
                if (handler.IsGenericTypeDefinition)
                {
                    this.CheckGenericDefinitionHandler<TRequest, TResponse>(handler, effectiveHandlerTypes);
                }
                else if (targetHandler == handler)
                {
                    effectiveHandlerTypes.Add(handler);
                }
                else
                {
                    this.logger.LogDebug($"Handler {handler.Name } can not be assigned to {targetHandler.Name}");
                }

            }

            if (effectiveHandlerTypes.Count == 0)
            {
                throw new ArgumentException($"No handler found for query type {typeof(TRequest).Name} and result {typeof(TResponse).Name}"); ;
            }

            if (effectiveHandlerTypes.Count > 1)
            {
                throw new ArgumentException($"Multiple handlers found for query type {typeof(TRequest).Name} and result {typeof(TResponse).Name}:\n {string.Join('\n', effectiveHandlerTypes.Select(h => h.FullName))}"); ;
            }

            var effectiveHandlerType = effectiveHandlerTypes.First();

            var effectiveHandler = this.provider.GetService(effectiveHandlerType);
            var e = effectiveHandler as IRequestHandler<TRequest, TResponse>;

            return e;
        }

        private void CheckGenericDefinitionHandler<TRequest, TResponse>(Type handler, List<Type> effectiveHandlers)
            where TRequest : IRequest<TResponse>
        {
            var rQueryType = typeof(TRequest);
            var rQueryOfDefinition = rQueryType.GetGenericTypeDefinition();
            var rSourceType = rQueryType.GetGenericArguments();

            var rQueryResultType = typeof(TResponse);
            var rQueryResultOfDefinition = rQueryResultType.GetGenericTypeDefinition();
            var rTargetType = rQueryResultType.GetGenericArguments();

            var iQueryHandlerInterface = handler.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
            if (iQueryHandlerInterface == null)
            {
                this.logger.LogWarning($"Registered handler {handler.Name} does not implement {typeof(IRequestHandler<,>)}, ignoring it");
                return;
            }

            var hQueryOf = iQueryHandlerInterface.GenericTypeArguments[0];
            var hQueryOfDefinition = hQueryOf.GetGenericTypeDefinition();


            var hQueryResultOf = iQueryHandlerInterface.GenericTypeArguments[1];
            var hQueryResultOfDefinition = hQueryResultOf.GetGenericTypeDefinition();
            
            if (rQueryOfDefinition == hQueryOfDefinition && rQueryResultOfDefinition == hQueryResultOfDefinition)
            {
                var hConcrete = handler.MakeGenericType(rSourceType);
                effectiveHandlers.Add(hConcrete);
            }
           
        }

    }

    public class RequestHandlerFactoryOptions
    {
        private readonly IList<Type> queryHandlers = new Collection<Type>();

        public IEnumerable<Type> QueryHandlers => new ReadOnlyCollection<Type>(this.queryHandlers);

        public void AddHandler(Type handlerType)
        {
            if (handlerType == null) throw new ArgumentNullException(nameof(handlerType));

            var t = typeof(IRequestHandler<,>);
            if (!handlerType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == t))
            {
                throw new ArgumentException($"The type {handlerType.Name} does not implements {t.Name}");
            }
            this.queryHandlers.Insert(0, handlerType);
        }
    }
}