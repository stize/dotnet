using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Stize.Persistence.QueryDispatcher
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly ILogger<QueryHandlerFactory> logger;
        private readonly QuerytHandlerFactoryOptions options;
        private readonly IServiceProvider provider;

        public QueryHandlerFactory(ILogger<QueryHandlerFactory> logger, IOptions<QuerytHandlerFactoryOptions> options, IServiceProvider provider)
        {
            this.logger = logger;
            this.options = options.Value;
            this.provider = provider;
        }

        public IQueryRequestHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IQueryRequest<TResponse>
        {
            var targetHandler = typeof(IQueryRequestHandler<TRequest, TResponse>);
            var effectiveHandlerTypes = new List<Type>();

            foreach (var handler in options.QueryHandlers)
            {
                if (handler.IsGenericTypeDefinition)
                {
                    CheckGenericDefinitionHandler<TRequest, TResponse>(handler, effectiveHandlerTypes);
                }
                else if (targetHandler == handler)
                {
                    effectiveHandlerTypes.Add(handler);
                }
                else
                {
                    logger.LogDebug($"Handler {handler.Name } can not be assigned to {targetHandler.Name}");
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

            var effectiveHandler = provider.GetService(effectiveHandlerType);
            var e = effectiveHandler as IQueryRequestHandler<TRequest, TResponse>;

            return e;
        }

        private void CheckGenericDefinitionHandler<TRequest, TResponse>(Type handler, List<Type> effectiveHandlers)
            where TRequest : IQueryRequest<TResponse>
        {
            var rQueryType = typeof(TRequest);
            var rQueryOfDefinition = rQueryType.GetGenericTypeDefinition();
            var rSourceType = rQueryType.GetGenericArguments();

            var rQueryResultType = typeof(TResponse);
            var rQueryResultOfDefinition = rQueryResultType.GetGenericTypeDefinition();
            var rTargetType = rQueryResultType.GetGenericArguments();

            var iQueryHandlerInterface = handler.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryRequestHandler<,>));
            if (iQueryHandlerInterface == null)
            {
                logger.LogWarning($"Registered handler {handler.Name} does not implement {typeof(IQueryRequestHandler<,>)}, ignoring it");
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
}