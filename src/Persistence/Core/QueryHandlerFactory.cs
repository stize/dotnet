using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stize.Persistence.Query;
using Stize.Persistence.QueryHandler;
using Stize.Persistence.QueryResult;

namespace Stize.Persistence
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly ILogger<QueryHandlerFactory> logger;
        private readonly QueryHandlerFactoryOptions options;
        private readonly IServiceProvider provider;

        public QueryHandlerFactory(ILogger<QueryHandlerFactory> logger, IOptions<QueryHandlerFactoryOptions> options, IServiceProvider provider)
        {
            this.logger = logger;
            this.provider = provider;
            this.options = options.Value;
        }

        public IQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>()
            where TQuery : IQuery
            where TResult : class, IQueryResult, new()
        {
            var targetHandler = typeof(IQueryHandler<TQuery, TResult>);
            var effectiveHandlers = new List<Type>();

            foreach (var handler in this.options.QueryHandlers)
            {
                if (handler.IsGenericTypeDefinition)
                {
                    this.CheckGenericDefinitionHandler<TQuery, TResult>(handler, effectiveHandlers);
                }
                else if (targetHandler == handler)
                {
                    effectiveHandlers.Add(handler);
                }
                else
                {
                    this.logger.LogDebug($"Handler {handler.Name } can not be assigned to {targetHandler.Name}");
                }

            }

            if (effectiveHandlers.Count == 0)
            {
                throw new ArgumentException($"No handler found for query type {typeof(TQuery).Name} and result {typeof(TResult).Name}"); ;
            }

            var hConcreteSelected = effectiveHandlers.OrderBy(x => x.GenericTypeArguments.Length).First();
            this.logger.LogDebug($"Selected {hConcreteSelected.Name} for handling {targetHandler.Name}");

            var service = this.provider.GetService(hConcreteSelected);
            var concreteHandlerInstance = service as IQueryHandler<TQuery, TResult>;
            return concreteHandlerInstance;


        }

        private void CheckGenericDefinitionHandler<TQuery, TResult>(Type handler, List<Type> effectiveHandlers)
            where TQuery : IQuery where
            TResult : class,
            IQueryResult, new()
        {
            var rQueryType = typeof(TQuery);
            var rQueryOfDefinition = rQueryType.GetGenericTypeDefinition();
            var rSourceType = rQueryType.GetGenericArguments().FirstOrDefault();

            var rQueryResultType = typeof(TResult);
            var rQueryResultOfDefinition = rQueryResultType.GetGenericTypeDefinition();
            var rTargetType = rQueryResultType.GetGenericArguments().FirstOrDefault();

            var iQueryHandlerInterface = handler.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            if (iQueryHandlerInterface == null)
            {
                this.logger.LogWarning($"Registered handler {handler.Name} does not implement {typeof(IQueryHandler<,>)}, ignoring it");
                return;
            }

            var hQueryOf = iQueryHandlerInterface.GenericTypeArguments[0];
            var hQueryOfDefinition = hQueryOf.GetGenericParameterConstraints()[0];


            var hQueryResultOf = iQueryHandlerInterface.GenericTypeArguments[1];
            var hQueryResultOfDefinition = hQueryResultOf.GetGenericParameterConstraints()[0];

            var x = hQueryOfDefinition.GetGenericTypeDefinition().MakeGenericType(rSourceType) == rQueryType;
            var y = hQueryResultOfDefinition.GetGenericTypeDefinition().MakeGenericType(rTargetType) == rQueryResultType;
            var z = rSourceType == rTargetType ? 3 : 4;

            var hParameters = handler.GetTypeInfo().GenericTypeParameters;

            if (x && y && hParameters.Length == z)
            {
                var hConcreteParameters = hParameters.Select(hp =>
                {
                    if (hp.Name == "TQuery") return rQueryType;
                    if (hp.Name == "TSource") return rSourceType;
                    if (hp.Name == "TTarget") return rTargetType;
                    if (hp.Name == "TResult") return rQueryResultType;
                    throw new ArgumentException("");
                }).ToArray();
                var hConcrete = handler.MakeGenericType(hConcreteParameters);
                effectiveHandlers.Add(hConcrete);
            }
        }
    }
}