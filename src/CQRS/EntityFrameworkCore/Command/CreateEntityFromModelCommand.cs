using Stize.CQRS.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class CreateEntityFromModelCommand<TModel, TEntity, TKey> : ICommand<Result<TKey>>
         where TModel : class
         where TEntity : class, IEntity<TKey>
    {
        public TModel Model { get; }

        public CreateEntityFromModelCommand(TModel model)
        {
            Model = model;
        }
    }
}
