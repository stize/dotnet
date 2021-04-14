using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Stize.Persistence.EntityFrameworkCore
{
    public class EfQueryableProvider : IQueryableProvider
    {

        public static IQueryableProvider Instance { get; } = new EfQueryableProvider();

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
