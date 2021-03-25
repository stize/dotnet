using Stize.CQRS.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Delta;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class PatchEntityFromModelCommand<TModel, TEntity, TKey> : ICommand<Result<TKey>>
         where TModel : class
         where TEntity : class, IEntity<TKey>
    {
        public Delta<TModel> Delta { get; }

        public PatchEntityFromModelCommand(Delta<TModel> delta)
        {
            Delta = delta;
        }
    }
}
