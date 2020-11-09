using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stize.Persistence.Repository.EntityFrameworkCore
{
    public class QueryableProvider : IQueryableProvider
    {
        private readonly IDbContextAccessor accessor;

        public QueryableProvider(IDbContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public IQueryable<T> GetQueryable<T>() where T : class
        {
            var context = this.accessor.GetContextByTargetEntityType<T>();
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
    }
}
