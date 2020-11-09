using System;
using System.Collections.Generic;
using System.Text;
using Stize.Persistence;
using Stize.Persistence.Materializer;
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

        private static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IQueryDispatcher), typeof(QueryDispatcher), ServiceLifetime.Scoped));
            return services;
        }

        private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(ISingleValueQueryHandler<,,>), typeof(SingleValueQueryHandler<,,>), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(ISingleValueQueryHandler<,>), typeof(SingleValueQueryHandler<,>), ServiceLifetime.Transient));

            services.Add(new ServiceDescriptor(typeof(IMultipleValueQueryHandler<,,>), typeof(MultipleValueQueryHandler<,,>), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IMultipleValueQueryHandler<,>), typeof(MultipleValueQueryHandler<,>), ServiceLifetime.Transient));

            services.Add(new ServiceDescriptor(typeof(IPagedValueQueryHandler<,,>), typeof(PagedValueQueryHandler<,,>), ServiceLifetime.Transient));
            services.Add(new ServiceDescriptor(typeof(IPagedValueQueryHandler<,>), typeof(PagedValueQueryHandler<,>), ServiceLifetime.Transient));

            return services;
        }

        private static IServiceCollection AddMaterializer(this IServiceCollection services)
        {
            services.Add(new ServiceDescriptor(typeof(IMaterializer<>), typeof(SingleSourceMaterializer<>), ServiceLifetime.Singleton));
            return services;
        }
    }
}
