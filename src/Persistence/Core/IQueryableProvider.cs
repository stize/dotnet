using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Persistence
{
    public interface IQueryableProvider
    {
        Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
        Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
        Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
    }

    public class DefaultQueryableProvider : IQueryableProvider
    {
        private DefaultQueryableProvider()
        {
            
        }

        public static IQueryableProvider Instance { get; } = new DefaultQueryableProvider();

        public Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }

        public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return Task.FromResult(queryable.ToArray());
        }

        public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class
        {
            return Task.FromResult(queryable.Count());
        }
    }
}