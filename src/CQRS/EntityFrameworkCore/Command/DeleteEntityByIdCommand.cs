using Microsoft.EntityFrameworkCore;
using Stize.Domain.Entity;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class DeleteEntityByIdCommand<TEntity, TKey, TContext> : EntityCommand<TEntity, TKey, TContext>
         where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        public TKey Id { get; }

        public DeleteEntityByIdCommand(TKey id)
        {
            Id = id;
        }
    }
}
