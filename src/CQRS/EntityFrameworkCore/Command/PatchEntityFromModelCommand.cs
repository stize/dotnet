using Microsoft.EntityFrameworkCore;
using Stize.Domain.Entity;
using Stize.DotNet.Delta;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class PatchEntityFromModelCommand<TModel, TEntity, TKey, TContext> : EntityCommand<TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        public Delta<TModel> Delta { get; }

        public PatchEntityFromModelCommand(Delta<TModel> delta)
        {
            Delta = delta;
        }
    }
}
