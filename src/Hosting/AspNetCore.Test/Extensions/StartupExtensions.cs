using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Stize.Hosting.AspNetCore.Test.Extensions
{
    public static class StartupExtensions
    {
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public static IServiceCollection AddApi(this IServiceCollection services)
        { ;
            services.AddHttpContextAccessor();
            services.AddControllers()
                .AddApplicationPart(typeof(StartupExtensions).Assembly)
                .AddFileModel()
                .AddPageDescriptor()
                ;

            services.AddAuthorization();

            services.AddStizeAspNetCore();

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static IApplicationBuilder UseApi(this IApplicationBuilder app)
        {
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            return app;
        }
    }
}
