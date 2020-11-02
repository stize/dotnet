using Stize.DotNet.FileSystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddLocalFileSystem(this IServiceCollection services)
        {
            services.AddScoped<IFileSystem, LocalFileSystemSystem>();
            return services;
        }
    }
}