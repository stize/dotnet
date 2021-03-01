using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryHandler;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.InquiryDispatcher
{
    public class InquiryHandlerFactory : IInquiryHandlerFactory
    {
        private readonly ILogger<InquiryHandlerFactory> logger;
        private readonly InquiryHandlerFactoryOptions options;
        private readonly IServiceProvider provider;

        public InquiryHandlerFactory(ILogger<InquiryHandlerFactory> logger, IOptions<InquiryHandlerFactoryOptions> options, IServiceProvider provider)
        {
            this.logger = logger;
            this.options = options.Value;
            this.provider = provider;
        }

        public IInquiryHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>()
            where TRequest : IInquiry<TResponse>
            where TResponse : IInquiryResult
        {
            var targetHandler = typeof(IInquiryHandler<TRequest, TResponse>);
            var effectiveHandlerTypes = new List<Type>();

            foreach (var handler in options.InquiryHandlers)
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
                throw new ArgumentException($"No handler found for inquiry type {typeof(TRequest).Name} and result {typeof(TResponse).Name}"); ;
            }

            if (effectiveHandlerTypes.Count > 1)
            {
                throw new ArgumentException($"Multiple handlers found for inquiry type {typeof(TRequest).Name} and result {typeof(TResponse).Name}:\n {string.Join('\n', effectiveHandlerTypes.Select(h => h.FullName))}"); ;
            }

            var effectiveHandlerType = effectiveHandlerTypes.First();

            var effectiveHandler = provider.GetService(effectiveHandlerType);
            var e = effectiveHandler as IInquiryHandler<TRequest, TResponse>;

            return e;
        }

        private void CheckGenericDefinitionHandler<TInquiry, TResponse>(Type handler, List<Type> effectiveHandlers)
            where TInquiry : IInquiry<TResponse>
            where TResponse : IInquiryResult
        {
            var rInquiryType = typeof(TInquiry);
            var rInquiryOfDefinition = rInquiryType.GetGenericTypeDefinition();
            var rSourceType = rInquiryType.GetGenericArguments();

            var rInquiryResultType = typeof(TResponse);
            var rInquiryResultOfDefinition = rInquiryResultType.GetGenericTypeDefinition();
            var rTargetType = rInquiryResultType.GetGenericArguments();

            var iInquiryHandlerInterface = handler.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInquiryHandler<,>));
            if (iInquiryHandlerInterface == null)
            {
                logger.LogWarning($"Registered handler {handler.Name} does not implement {typeof(IInquiryHandler<,>)}, ignoring it");
                return;
            }

            var hInquiryOf = iInquiryHandlerInterface.GenericTypeArguments[0];
            var hInquiryOfDefinition = hInquiryOf.GetGenericTypeDefinition();


            var hInquiryResultOf = iInquiryHandlerInterface.GenericTypeArguments[1];
            var hInquiryResultOfDefinition = hInquiryResultOf.GetGenericTypeDefinition();

            if (rInquiryOfDefinition == hInquiryOfDefinition && rInquiryResultOfDefinition == hInquiryResultOfDefinition)
            {
                var hConcrete = handler.MakeGenericType(rSourceType);
                effectiveHandlers.Add(hConcrete);
            }

        }

    }
}