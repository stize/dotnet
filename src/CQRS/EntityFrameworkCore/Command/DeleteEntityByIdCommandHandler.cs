using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stize.CQRS.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Specification;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class DeleteEntityByIdCommandHandler<TEntity, TKey, TContext> : ICommandHandler<DeleteEntityByIdCommand<TEntity, TKey>, Result<TKey>>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly ILogger<DeleteEntityByIdCommandHandler<TEntity, TKey, TContext>> logger;
        private readonly IEntityRepository<TContext> repository;

        public DeleteEntityByIdCommandHandler(ILogger<DeleteEntityByIdCommandHandler<TEntity, TKey, TContext>> logger, IEntityRepository<TContext> repository)
        {
            this.logger = logger;
            this.repository = repository;
        }
        public async Task<Result<TKey>> HandleAsync(DeleteEntityByIdCommand<TEntity, TKey> request, CancellationToken cancellationToken = default)
        {
            var exists = await this.repository.Where(new EntityByIdSpecification<TEntity, TKey>(request.Id)).AnyAsync(cancellationToken);
            if (!exists)
            {
                var error = $"The entity of type {typeof(TEntity).Name} with key {request.Id} does not exits and can not be updated";
                this.logger.LogDebug(error);
                return Result<TKey>.Fail(error);
            }

            await this.repository.RemoveAsync<TEntity, TKey>(request.Id, cancellationToken);
            await this.repository.CommitAsync(cancellationToken);

            return Result<TKey>.Success(request.Id);
        }
    }
}
