using System;
using Microsoft.EntityFrameworkCore;
using Stize.Persistence.Repository.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizeEntityFrameworkRepository<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null) where TContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddDbContext<TContext>(optionsAction, ServiceLifetime.Scoped);
            services.AddDbContext(typeof(TContext), ServiceLifetime.Scoped);
            services.AddDbContextAccessor(ServiceLifetime.Scoped);
            services.AddEEntityRepository(ServiceLifetime.Scoped);
            
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

        public static IServiceCollection AddEEntityRepository(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(IEntityRepository<>), typeof(EntityRepository<>), lifetime));
            services.Add(new ServiceDescriptor(typeof(IEntityRepository<,>), typeof(EntityRepository<,>), lifetime));
            return services;
        }
        
    }
}