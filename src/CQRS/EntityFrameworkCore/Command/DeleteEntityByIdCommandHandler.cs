using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Specification;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class DeleteEntityByIdCommandHandler<TEntity, TKey, TContext> :
        EntityCommandHandler<DeleteEntityByIdCommand<TEntity, TKey, TContext>, TEntity, TKey, TContext>
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
        public override async Task<Result<TKey>> HandleAsync(DeleteEntityByIdCommand<TEntity, TKey, TContext> request, CancellationToken cancellationToken = default)
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
