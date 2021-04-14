using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stize.Domain;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Materializer;
using Stize.Persistence.Specification;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class UpdateEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext> :
        EntityCommandHandler<UpdateEntityFromModelCommand<TModel, TEntity, TKey, TContext>, TModel, TEntity, TKey, TContext>
        where TModel : class, IObject<TKey>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly ILogger<UpdateEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext>> logger;
        private readonly IEntityRepository<TContext> repository;
        private readonly IEntityBuilder<TEntity, TKey> entityBuilder;

        public UpdateEntityFromModelCommandHandler(
            ILogger<UpdateEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext>> logger, 
            IEntityRepository<TContext> repository,
            IEntityBuilder<TEntity, TKey> entityBuilder)
        {
            this.logger = logger;
            this.repository = repository;
            this.entityBuilder = entityBuilder;
        }
        public override async Task<Result<TKey>> HandleAsync(UpdateEntityFromModelCommand<TModel, TEntity, TKey, TContext> request, CancellationToken cancellationToken = default)
        {
            var exists = await this.repository.Where(new EntityByIdSpecification<TEntity, TKey>(request.Model.Id)).AnyAsync(cancellationToken);
            if (!exists)
            {
                var error = $"The entity of type {typeof(TEntity).Name} with key {request.Model.Id} does not exits and can not be updated";
                this.logger.LogDebug(error);
                return Result<TKey>.Fail(error);
            }

            var entity = await this.entityBuilder.UpdateAsync(request.Model);
            await this.repository.CommitAsync(cancellationToken);

            return Result<TKey>.Success(entity.Id);

        }
    }
}
