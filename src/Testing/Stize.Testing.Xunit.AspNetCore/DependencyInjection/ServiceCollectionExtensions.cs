using Microsoft.Extensions.DependencyInjection;
using Stize.Testing.Xunit.AspNetCore;
using Stize.Testing.Xunit.AspNetCore.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestServerAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Constants.Scheme;
            }).AddTestServer();

            return services;
        }
    }
}