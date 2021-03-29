using System;
using Stize.Mediator.Internal;
using Stize.Persistence.InquiryHandler;
using Stize.Persistence.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizePersistence(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandlerFactory, InquiryHandlerFactory>();
            services.AddInquiryHandlers();
            return services;
        }

       
        private static IServiceCollection AddInquiryHandlers(this IServiceCollection services)
        {
            services.AddScoped(typeof(IInquiryHandler<,>), typeof(SingleValueInquiryHandler<,>));
            services.AddScoped(typeof(IInquiryHandler<,>), typeof(MultipleValueInquiryHandler<,>));
            services.AddScoped(typeof(IInquiryHandler<,>), typeof(PagedValueInquiryHandler<,>));

            return services;
        }
    }
}
