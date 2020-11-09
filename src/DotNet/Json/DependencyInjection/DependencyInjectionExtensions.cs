using System.Text.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stize.DotNet.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTextJson(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializer, TextJsonSerializer>();
            services.TryAddSingleton(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return services;
        }

        public static IServiceCollection AddTextJson(this IServiceCollection services, JsonSerializerOptions settings)
        {
            services.AddSingleton<IJsonSerializer, TextJsonSerializer>();
            services.TryAddSingleton(settings);
            return services;
        }
    }
}