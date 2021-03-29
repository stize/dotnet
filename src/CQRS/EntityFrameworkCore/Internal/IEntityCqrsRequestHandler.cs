using Microsoft.EntityFrameworkCore;
using Stize.Domain;
using Stize.Domain.Entity;

namespace Stize.CQRS.EntityFrameworkCore.Internal
{
    internal interface IEntityCqrsRequestHandler<TEntity, TKey, TContext> 
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
    }

    internal interface IEntityCqrsRequestHandler<TModel, TEntity, TKey, TContext> : IEntityCqrsRequestHandler<TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
    }
}
