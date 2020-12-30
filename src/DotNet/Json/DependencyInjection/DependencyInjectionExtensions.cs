using System.Text.Json;
using Stize.DotNet.Json;
using Stize.DotNet.Json.Converter;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTextJson(this IServiceCollection services)
        {

            services.AddSingleton<IJsonSerializer, TextJsonSerializer>();
            services.PostConfigure<JsonSerializerOptions>(opt => opt.Converters.Add(new DeltaOfJsonConverterFactory()));

            return services;
        }

    }
}