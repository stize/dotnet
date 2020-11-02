using System;
using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Providers.DateTime;
using Stize.DotNet.Providers.Identity;

namespace Microsoft.Extensions.DependencyInjection {

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
        {
            return services.AddDateTimeProvider(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddDateTimeProvider(this IServiceCollection services, ServiceLifetime lifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(typeof(IDateTimeProvider), typeof(DateTimeProvider), lifetime));

            return services;
        }

        public static IServiceCollection AddIdentityProvider(this IServiceCollection services)
        {
            return services.AddIdentityProvider(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddIdentityProvider(this IServiceCollection services, ServiceLifetime lifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(typeof(IIdentityProvider), typeof(IdentityProvider), lifetime));

            return services;
        }
    }
}