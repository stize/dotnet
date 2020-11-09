using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Stize.Domain;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Repository.EntityFrameworkCore
{
    public class EntityRepository<TContext> : IEntityRepository<TContext>
    where TContext : DbContext
    {
        private readonly IDbContextAccessor accessor;

        public TContext Context => this.accessor.GetCurrentContext<TContext>();

        private IDbContextTransaction Tx => this.Context.Database.CurrentTransaction;

        public EntityRepository(IDbContextAccessor accessor)
        {
            this.accessor = accessor;
        }
        
        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            return this.GetQuery<T>();
        }

        public virtual T Add<T>(T entity) where T : class
        {
            var entry = this.Context.Set<T>().Add(entity);
            return entry.Entity;
        }

        public virtual void Remove<T>(T entity) where T : class
        {
            this.Context.Set<T>().Remove(entity);
        }
        
        public virtual IQueryable<T> Where<T>(ISpecification<T> specification) where T : class
        {
            return this.GetQuery<T>().Where(specification.Predicate);
        }


        public virtual void BeginTransaction()
        {
            if (this.Tx == null)
            {
                this.Context.Database.BeginTransaction();
            }
        }

        public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (this.Tx == null)
            {
                await this.Context.Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public virtual void Commit()
        {
            this.Context.SaveChanges();


            this.Tx?.Commit();
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await this.Context.SaveChangesAsync(cancellationToken);

            if (this.Tx != null)
            {
                await this.Tx.CommitAsync(cancellationToken);
            }

        }

        public virtual void Rollback()
        {
            this.Tx?.Rollback();
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (this.Tx != null)
            {
                await this.Tx.RollbackAsync(cancellationToken);
            }

        }


        protected virtual IQueryable<T> GetQuery<T>() where T : class
        {
            return this.Context.Set<T>();
        }

    }


    public class EntityRepository<TContext, TKey> : EntityRepository<TContext>, IEntityRepository<TContext, TKey>
        where TContext : DbContext
    {
        public EntityRepository(IDbContextAccessor accessor) : base(accessor)
        {
        }

        public virtual T FindOne<T>(TKey key) where T : class, IEntity<TKey>
        {
            var queryable = this.GetQuery<T>()
                .Where(ExpressionExtensions.Equals<T, TKey>(ExpressionExtensions.GetPropertyName<T, TKey>(e => e.Id), key));
            return queryable.FirstOrDefault();
        }

        public virtual Task<T> FindOneAsync<T>(TKey key, CancellationToken cancellationToken = default) where T : class, IEntity<TKey>
        {
            var queryable = this.GetQuery<T>()
                .Where(ExpressionExtensions.Equals<T, TKey>(ExpressionExtensions.GetPropertyName<T, TKey>(e => e.Id), key));
            return queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual void Remove<T>(TKey key) where T : class, IEntity<TKey>
        {
            var entity = this.FindOne<T>(key);
            if (entity == null)
            {
                throw new ArgumentException(nameof(key));
            }
            this.Remove(entity);
        }

        public async Task RemoveAsync<T>(TKey key, CancellationToken cancellationToken = default) where T : class, IEntity<TKey>
        {
            var entity = await this.FindOneAsync<T>(key, cancellationToken);
            if (entity == null)
            {
                throw new ArgumentException(nameof(key));
            }
            this.Remove(entity);
        }
    }
}