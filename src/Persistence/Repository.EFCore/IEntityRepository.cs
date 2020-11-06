﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stize.Domain;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Repository.EntityFrameworkCore
{
    public interface IEntityRepository<out TContext>
        where TContext : DbContext
    {
        public TContext Context { get; }

        public IQueryable<T> GetAll<T>() where T : class;

        public T Add<T>(T entity) where T : class;

        public void Remove<T>(T entity) where T : class;

        public IQueryable<T> Where<T>(ISpecification<T> specification) where T : class;

        public void BeginTransaction();

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        public void Commit();

        public Task CommitAsync(CancellationToken cancellationToken = default);

        public void Rollback();

        public Task RollbackAsync(CancellationToken cancellationToken = default);
    }

    public interface IEntityRepository<out TContext, TKey>: IEntityRepository<TContext>
        where TContext : DbContext
    {
        public T FindOne<T>(TKey key) where T : class, IEntity<TKey>;

        public Task<T> FindOneAsync<T>(TKey key, CancellationToken cancellationToken = default) where T : class, IEntity<TKey>;

        public void Remove<T>(TKey key) where T : class, IEntity<TKey>;

        public Task RemoveAsync<T>(TKey key, CancellationToken cancellationToken = default) where T : class, IEntity<TKey>;
    }
}