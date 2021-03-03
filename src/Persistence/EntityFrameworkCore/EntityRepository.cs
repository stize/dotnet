using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Stize.Domain;
using Stize.DotNet.Specification;
using Stize.Persistence.Inquiry;
using Stize.Persistence.InquiryDispatcher;
using Stize.Persistence.InquiryResult;

namespace Stize.Persistence.EntityFrameworkCore
{
    public class EntityRepository<TContext> : IEntityRepository<TContext>
    where TContext : DbContext
    {
        private readonly TContext context;

        private readonly IInquiryDispatcher queryDispatcher;

        private IDbContextTransaction Tx => this.context.Database.CurrentTransaction;

        public EntityRepository(TContext dbContext, IInquiryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
            this.context = dbContext;
        }

        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            return this.GetQuery<T>();
        }

        public virtual T Add<T>(T entity) where T : class
        {
            var entry = this.context.Set<T>().Add(entity);
            return entry.Entity;
        }

        public virtual void Remove<T>(T entity) where T : class
        {
            this.context.Set<T>().Remove(entity);
        }

        public virtual IQueryable<T> Where<T>(ISpecification<T> specification) where T : class
        {
            return this.GetQuery<T>().Where(specification.Predicate);
        }


        public virtual async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (this.Tx == null)
            {
                await this.context.Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await this.context.SaveChangesAsync(cancellationToken);

            if (this.Tx != null)
            {
                await this.Tx.CommitAsync(cancellationToken);
            }

        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (this.Tx != null)
            {
                await this.Tx.RollbackAsync(cancellationToken);
            }

        }

        public virtual Task<T> FindOneAsync<T, TKey>(TKey key, CancellationToken cancellationToken = default)
            where T : class, IObject<TKey>
        {
            var queryable = this.GetQuery<T>().Where(ExpressionExtensions.Equals<T, TKey>(ExpressionExtensions.GetPropertyName<T, TKey>(e => e.Id), key));
            return queryable.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task RemoveAsync<T, TKey>(TKey key, CancellationToken cancellationToken = default)
            where T : class, IObject<TKey>
        {
            var entity = await this.FindOneAsync<T, TKey>(key, cancellationToken);
            if (entity == null)
            {
                throw new ArgumentException(nameof(key));
            }
            this.Remove(entity);

        }

        public virtual Task<TResult> RunQueryAsync<TSource, TTarget, TResult>(IInquiry<TSource, TTarget, TResult> query, CancellationToken cancellationToken = default)
            where TSource : class
            where TTarget : class
            where TResult : class, IInquiryResult<TTarget>
        {
            query.Provider = EfQueryableProvider.Instance;
            query.SourceQuery = this.GetQuery<TSource>();
            return this.queryDispatcher.HandleAsync(query, cancellationToken);
        }


        protected virtual IQueryable<T> GetQuery<T>() where T : class
        {
            return this.context.Set<T>();
        }

    }
}