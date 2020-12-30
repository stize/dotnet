using System;
using System.Reflection;
using Stize.Mapping.Service;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        
        public static IServiceCollection AddStizeMapperService(this IServiceCollection services)
        {
            services.AddMapperService(ServiceLifetime.Scoped);
            return services;
        }

        public static IServiceCollection AddMapperService(this IServiceCollection services, ServiceLifetime lifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(typeof(IMappingService<>), typeof(MappingService<>), lifetime));
            return services;
        }
    }
}