using Stize.DotNet.Result;
using Stize.Mediator;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizeMediator(this IServiceCollection services)
        {
            services.AddTransient<IMediator, Mediator>();
            return services;
        }

        public static IServiceCollection AddStizeMediatorHandler<THandler>(this IServiceCollection services)
             where THandler : class
        {
            var handlerType = typeof(THandler);
            return services.AddStizeMediatorHandler(handlerType);
        }

        public static IServiceCollection AddStizeMediatorHandler(this IServiceCollection services, Type handlerType)
        {
            var interfaces = handlerType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)).ToArray();
            foreach (var i in interfaces)
            {
                services.AddTransient(i, handlerType);
            }


            return services;
        }
    }
}
