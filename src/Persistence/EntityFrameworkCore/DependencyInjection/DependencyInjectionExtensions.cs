using System;
using Microsoft.EntityFrameworkCore;
using Stize.Persistence;
using Stize.Persistence.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizeEntityDbContext<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null) where TContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddDbContext<TContext>(optionsAction, ServiceLifetime.Scoped);
            services.AddDbContext(typeof(TContext), ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddStizeEntityRepository(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddDbContextAccessor(ServiceLifetime.Scoped);
            services.AddEntityRepository(ServiceLifetime.Scoped);
            services.AddQueryableProvider(ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, Type dbContextType, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(DbContext), provider => provider.GetService(dbContextType), lifetime));
            return services;
        }

        public static IServiceCollection AddDbContextAccessor(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(IDbContextAccessor), typeof(DbContextAccessor), lifetime));
            return services;
        }

        public static IServiceCollection AddEntityRepository(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(IEntityRepository<>), typeof(EntityRepository<>), lifetime));
            return services;
        }

        public static IServiceCollection AddQueryableProvider(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(IQueryableProvider), typeof(QueryableProvider), lifetime));
            return services;
        }

    }
}