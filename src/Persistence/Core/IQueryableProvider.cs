using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.Persistence
{
    public interface IQueryableProvider
    {
        IQueryable<T> GetQueryable<T>() where T : class;

        Task<T> SingleOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
        Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
        Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) where T : class;
    }
}