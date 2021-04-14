using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.CQRS.EntityFrameworkCore.Internal;
using Stize.CQRS.EntityFrameworkCore.Query;
using Stize.Mediator.Internal;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddStizeCqrsEntityFrameworkCore(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandlerFactory, EntityCqrsRequestHandlerFactory>();
            services.AddStizeEntityQueryHandler();
            services.AddStizeEntityCommandHandler();
            return services;
        }

        public static IServiceCollection AddStizeEntityQueryHandler(this IServiceCollection services)
        {
            services.AddStizeCqrsRequestHandler(typeof(GetAllModelsFromEntityByPageQueryHandler<,,,>));
            services.AddStizeCqrsRequestHandler(typeof(GetAllModelsFromEntityQueryHandler<,,,>));
            services.AddStizeCqrsRequestHandler(typeof(GetModelFromEntityByIdQueryHandler<,,,>));
            return services;
        }

        public static IServiceCollection AddStizeEntityCommandHandler(this IServiceCollection services)
        {
            services.AddStizeCqrsRequestHandler(typeof(CreateEntityFromModelCommandHandler<,,,>));
            services.AddStizeCqrsRequestHandler(typeof(UpdateEntityFromModelCommandHandler<,,,>));
            services.AddStizeCqrsRequestHandler(typeof(PatchEntityFromModelCommandHandler<,,,>));
            services.AddStizeCqrsRequestHandler(typeof(DeleteEntityByIdCommandHandler<,,>));
            return services;
        }

        public static IServiceCollection AddStizeCqrsRequestHandler(this IServiceCollection services, Type handlerType)
        {
            var parameters = handlerType.GetTypeInfo().GenericTypeParameters;
            if (parameters.Length == 3)
            {
                services.AddScoped(typeof(IEntityCqrsRequestHandler<,,>), handlerType);
            }
            else if(parameters.Length == 4)
            {
                services.AddScoped(typeof(IEntityCqrsRequestHandler<,,,>), handlerType);
            }
            
            return services;
        }
    }
}
