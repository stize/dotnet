using Microsoft.EntityFrameworkCore;
using Stize.CQRS.Command;
using Stize.CQRS.EntityFrameworkCore.Internal;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public abstract class EntityCommandHandler<TCommand, TEntity, TKey, TContext> :
       ICommandHandler<TCommand, Result<TKey>>,
       IEntityCqrsRequestHandler<TEntity, TKey, TContext>
       where TCommand : ICommand<Result<TKey>>
       where TEntity : class, IEntity<TKey>
       where TContext : DbContext
    {
        public abstract Task<Result<TKey>> HandleAsync(TCommand request, CancellationToken cancellationToken = default);
    }

    public abstract class EntityCommandHandler<TCommand, TModel, TEntity, TKey, TContext> :
       ICommandHandler<TCommand, Result<TKey>>,
       IEntityCqrsRequestHandler<TModel, TEntity, TKey, TContext>
       where TCommand : ICommand<Result<TKey>>
       where TModel : class
       where TEntity : class, IEntity<TKey>
       where TContext : DbContext
    {
        public abstract Task<Result<TKey>> HandleAsync(TCommand request, CancellationToken cancellationToken = default);
    }
}
