using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Internal;
using Stize.CQRS.Query;
using Stize.Domain.Entity;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class EntityQuery<TResult, TModel, TEntity, TKey, TContext> : IQuery<TResult>, IEntityCqrsRequest<TModel, TEntity, TKey, TContext>
        where TResult : IValueResult<TModel>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {

    }
}
