using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Stize.Persistence.EntityFrameworkCore
{
    public class QueryableProvider : IQueryableProvider
    {
        private readonly IServiceProvider serviceProvider;

        public QueryableProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            var context = this.GetContextByTargetEntityType<T>();
            var queryable = context.Set<T>();
            return queryable;
        }

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return queryable.SingleOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return queryable.ToArrayAsync(cancellationToken: cancellationToken);
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return queryable.CountAsync(cancellationToken: cancellationToken);
        }

        private DbContext GetContextByTargetEntityType<T>() where T : class
        {
            var contexts = this.serviceProvider.GetServices<DbContext>();
            var context = contexts.FirstOrDefault(x => x.Model.FindEntityType(typeof(T)) != null);
            return context;
        }
    }
}
