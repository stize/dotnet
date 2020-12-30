using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stize.DotNet.Providers.Identity;
using Stize.Hosting.AspNetCore.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizeAspNetCore(this IServiceCollection services)
        {
            services.AddHttpContextPrincipal();
            services.AddTextJson();
            services.PostConfigure<JsonSerializerOptions>(opt => opt.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
            return services;
        }

        public static IServiceCollection AddHttpContextPrincipal(this IServiceCollection services)
        {
            return services.AddHttpContextPrincipal(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddHttpContextPrincipal(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(IPrincipalProvider), typeof(PrincipalProvider), lifetime));
            return services;
        }

        public static IMvcBuilder AddFileModel(this IMvcBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureFileModelMvcOptions>();
            return builder;
        }
        
        public static IMvcBuilder AddPageDescriptor(this IMvcBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigurePageDescriptorMvcOptions>();
            return builder;
        }
       
    }

}