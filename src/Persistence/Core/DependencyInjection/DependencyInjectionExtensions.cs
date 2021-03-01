using System;
using Stize.Persistence.InquiryDispatcher;
using Stize.Persistence.InquiryHandler;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizePersistence(this IServiceCollection services)
        {
            services.AddInquiryDispatcher();
            services.AddInquiryHandlers();
            return services;
        }

        public static IServiceCollection AddInquiryHandler(this IServiceCollection services, Type handlerType)
        {
            services.Configure<InquiryHandlerFactoryOptions>(options => options.AddHandler(handlerType));
            services.AddTransient(handlerType);
            return services;
        }

        private static IServiceCollection AddInquiryDispatcher(this IServiceCollection services)
        {
            services.AddScoped<IInquiryDispatcher, InquiryDispatcher>()
                    .AddScoped<IInquiryHandlerFactory, InquiryHandlerFactory>();
            return services;
        }

        private static IServiceCollection AddInquiryHandlers(this IServiceCollection services)
        {
            services.AddInquiryHandler(typeof(SingleValueInquiryHandler<,>));
            services.AddInquiryHandler(typeof(MultipleValueInquiryHandler<,>));
            services.AddInquiryHandler(typeof(PagedValueInquiryHandler<,>));

            return services;
        }
    }
}
