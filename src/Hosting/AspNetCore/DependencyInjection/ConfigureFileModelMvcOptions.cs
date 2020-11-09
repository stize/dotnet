using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stize.Hosting.AspNetCore.Formatting;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ConfigureFileModelMvcOptions : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.InputFormatters.Insert(0, new MultiUploadedFileModelFormatter());
        }
    }

    public class ConfigureFileModelMvcOptions<T> : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.InputFormatters.Insert(0, new MultiUploadedFileModelFormatter<T>());
        }
    }
}