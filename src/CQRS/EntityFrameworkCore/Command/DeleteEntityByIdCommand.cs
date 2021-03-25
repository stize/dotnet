using Stize.CQRS.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class DeleteEntityByIdCommand<TEntity, TKey> : ICommand<Result<TKey>>         
         where TEntity : class, IEntity<TKey>
    {
        public TKey Id { get; }

        public DeleteEntityByIdCommand(TKey id)
        {
            Id = id;
        }
    }
}
