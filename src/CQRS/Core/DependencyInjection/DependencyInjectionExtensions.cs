using Stize.CQRS.Command;
using Stize.CQRS.Query;
using Stize.CQRS.Saga;
using Stize.DotNet.Result;
using Stize.Mediator;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static IServiceCollection AddStizeCommandHandler<THandler, TCommand, TResult>(this IServiceCollection services)
            where THandler : ICommandHandler<TCommand, TResult>
            where TCommand : ICommand<TResult>
            where TResult : IValueResult
        {
            var handlerType = typeof(THandler);
            var interfaces = handlerType.GetInterfaces()
                                        .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                                        .ToArray();
            foreach (var i in interfaces)
            {
                services.AddTransient(i, handlerType);
            }

            return services;
        }

        public static IServiceCollection AddStizeQueryHandler<THandler, TQuery, TResult>(this IServiceCollection services)
            where THandler : IQueryHandler<TQuery, TResult>
            where TQuery : IQuery<TResult>
            where TResult : IValueResult
        {
            var handlerType = typeof(THandler);
            var interfaces = handlerType.GetInterfaces()
                                        .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                                        .ToArray();
            foreach (var i in interfaces)
            {
                services.AddTransient(i, handlerType);
            }
            return services;
        }

        public static IServiceCollection AddStizeSagaHandler<THandler, TSaga, TResult>(this IServiceCollection services)
            where THandler : ISagaHandler<TSaga, TResult>
            where TSaga : ISaga<TResult>
            where TResult : IValueResult
        {
            var handlerType = typeof(THandler);
            var interfaces = handlerType.GetInterfaces()
                                        .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ISagaHandler<,>)) || i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                                        .ToArray();
            foreach (var i in interfaces)
            {
                services.AddTransient(i, handlerType);
            }
            return services;
        }

    }
}
