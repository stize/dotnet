using Microsoft.EntityFrameworkCore;
using Stize.Domain.Entity;

namespace Stize.CQRS.EntityFrameworkCore.Internal
{
    internal interface IEntityCqrsRequest<TEntity, TKey, TContext>
       where TEntity : class, IEntity<TKey>
       where TContext : DbContext
    { }

    internal interface IEntityCqrsRequest<TModel, TEntity, TKey, TContext> : IEntityCqrsRequest<TEntity, TKey, TContext>
    where TModel : class
    where TEntity : class, IEntity<TKey>
    where TContext : DbContext
    { }
}
