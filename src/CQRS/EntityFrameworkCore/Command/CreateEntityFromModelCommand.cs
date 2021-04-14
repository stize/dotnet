using Microsoft.EntityFrameworkCore;
using Stize.Domain.Entity;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class CreateEntityFromModelCommand<TModel, TEntity, TKey, TContext> : EntityCommand<TModel, TEntity, TKey, TContext>
         where TModel : class
         where TEntity : class, IEntity<TKey>
         where TContext : DbContext
    {
        public TModel Model { get; }

        public CreateEntityFromModelCommand(TModel model)
        {
            Model = model;
        }
    }
}
