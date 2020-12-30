using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stize.Domain;
using Stize.DotNet.Specification;

namespace Stize.Persistence.EntityFrameworkCore
{
    public interface IEntityRepository<out TContext>
        where TContext : DbContext
    {
        public TContext Context { get; }

        public IQueryable<T> GetAll<T>() where T : class;

        public T Add<T>(T entity) where T : class;

        public void Remove<T>(T entity) where T : class;

        public IQueryable<T> Where<T>(ISpecification<T> specification) where T : class;

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        public Task CommitAsync(CancellationToken cancellationToken = default);

        public Task RollbackAsync(CancellationToken cancellationToken = default);

        public Task<T> FindOneAsync<T, TKey>(TKey key, CancellationToken cancellationToken = default) where T : class, IObject<TKey>;

        public Task RemoveAsync<T, TKey>(TKey key, CancellationToken cancellationToken = default) where T : class, IObject<TKey>;
    }

}