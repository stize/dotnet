using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stize.Hosting.AspNetCore.ModelBinding;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ConfigurePageDescriptorMvcOptions : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new PageDescriptorModelBinderProvider());
        }
    }
}