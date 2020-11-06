using System;
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
    }
}