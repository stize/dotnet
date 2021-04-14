using Microsoft.EntityFrameworkCore;
using Stize.CQRS.Command;
using Stize.CQRS.EntityFrameworkCore.Internal;
using Stize.Domain.Entity;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class EntityCommand<TEntity, TKey, TContext> : ICommand<Result<TKey>>, IEntityCqrsRequest<TEntity, TKey, TContext>
         where TEntity : class, IEntity<TKey>
         where TContext : DbContext
    {

    }

    public class EntityCommand<TModel, TEntity, TKey, TContext> : ICommand<Result<TKey>>, IEntityCqrsRequest<TModel, TEntity, TKey, TContext>
         where TModel : class
         where TEntity : class, IEntity<TKey>
         where TContext : DbContext
    {

    }
}
