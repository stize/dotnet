using System;
using System.Collections.Generic;
using System.Text;
using Stize.Persistence;
using Stize.Persistence.Materializer;
using Stize.Persistence.QueryDispatcher;
using Stize.Persistence.QueryHandler;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizePersistence(this IServiceCollection services)
        {
            services.AddQueryDispatcher();
            services.AddQueryHandlers();
            services.AddMaterializer();
            return services;
        }

        public static IServiceCollection AddQueryHandler(this IServiceCollection services, Type handlerType)
        {
            services.Configure<QuerytHandlerFactoryOptions>(options => options.AddHandler(handlerType));
            services.AddTransient(handlerType);
            return services;
        }

        private static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
        {
            services.AddScoped<IQueryDispatcher, QueryDispatcher>()
                    .AddScoped<IQueryHandlerFactory, QueryHandlerFactory>();
            return services;
        }

        private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler(typeof(SingleValueQueryHandler<,>));
            services.AddQueryHandler(typeof(SingleValueQueryHandler<>));

            services.AddQueryHandler(typeof(MultipleValueQueryHandler<,>));
            services.AddQueryHandler(typeof(MultipleValueQueryHandler<>));

            services.AddQueryHandler(typeof(PagedValueQueryHandler<,>));
            services.AddQueryHandler(typeof(PagedValueQueryHandler<>));

            return services;
        }

        private static IServiceCollection AddMaterializer(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IMaterializer<>), typeof(SingleSourceMaterializer<>), ServiceLifetime.Scoped));
            return services;
        }
    }
}
