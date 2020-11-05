using System;
using System.Reflection;
using AutoMapper;
using Stize.Mapping.AutoMapper;
using IObjectMapper = Stize.Mapping.IObjectMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Creates a <see cref="AutoMapperObjectMapper" /> Scoped <see cref="ServiceLifetime" /> service to the given
        /// <see cref="IServiceCollection" />
        /// </summary>
        /// <param name="services">Services to add the scoped <see cref="AutoMapperObjectMapper" /></param>
        /// <param name="profileAssembly"><see cref="Assembly" /> to scan for AutoMapper <see cref="Profile" /></param>
        /// <returns>Configured <paramref name="services" /></returns>
        public static IServiceCollection AddStizeAutoMapper(this IServiceCollection services, Assembly profileAssembly)
        {
            services.AddObjectMapper(ServiceLifetime.Scoped);
            services.AddAutoMapper(profileAssembly);
            return services;
        }

        /// <summary>
        /// Creates a <a <see cref="AutoMapperObjectMapper" /> service with the given <see cref="ServiceLifetime" />
        /// </summary>
        /// <param name="services">Services to add the scoped <see cref="AutoMapperObjectMapper" /></param>
        /// <param name="lifetime">Object instance lifespan</param>
        /// <exception cref="ArgumentNullException">If <paramref name="services" /> is null</exception>
        /// <returns>Configured <paramref name="services" /></returns>
        public static IServiceCollection AddObjectMapper(this IServiceCollection services, ServiceLifetime lifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(typeof(IObjectMapper), typeof(AutoMapperObjectMapper), lifetime));
            return services;
        }
    }
}