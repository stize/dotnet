using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Stize.Persistence.Repository.EntityFrameworkCore
{
    public class DbContextAccessor : IDbContextAccessor
    {
        private readonly IServiceProvider serviceProvider;

        public DbContextAccessor(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TContext GetCurrentContext<TContext>() where TContext : DbContext
        {
            return this.serviceProvider.GetRequiredService<TContext>();
        }

        public DbContext GetContextByTargetEntityType<T>() where T : class
        {
            var contexts = this.serviceProvider.GetServices<DbContext>();
            var context = contexts.FirstOrDefault(x => x.Model.FindEntityType(typeof(T)) != null);
            return context;
        }
    }
}