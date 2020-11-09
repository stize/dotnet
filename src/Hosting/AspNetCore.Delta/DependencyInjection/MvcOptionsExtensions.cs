using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stize.Hosting.AspNetCore.Delta.ModelBinding;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcOptionsExtensions
    {

        public static IMvcBuilder AddDelta(this IMvcBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureDeltaMvcOptions>();
            return builder;
        }

        public static IMvcCoreBuilder AddDelta(this IMvcCoreBuilder builder)
        {
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureDeltaMvcOptions>();
            return builder;
        }
    }

    public class ConfigureDeltaMvcOptions : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new DeltaModelBinderProvider());
        }
    }
}
