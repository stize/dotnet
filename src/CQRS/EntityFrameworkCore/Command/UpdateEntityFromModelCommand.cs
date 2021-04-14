using Microsoft.EntityFrameworkCore;
using Stize.Domain;
using Stize.Domain.Entity;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class UpdateEntityFromModelCommand<TModel, TEntity, TKey, TContext> : EntityCommand<TModel, TEntity, TKey, TContext>
        where TModel : class, IObject<TKey>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        public TModel Model { get; }

        public UpdateEntityFromModelCommand(TModel model)
        {
            Model = model;
        }
    }
}
