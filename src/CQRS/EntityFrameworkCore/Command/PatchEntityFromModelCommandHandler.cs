using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stize.Domain;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Materializer;
using Stize.Persistence.Specification;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class PatchEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext> :
        EntityCommandHandler<PatchEntityFromModelCommand<TModel, TEntity, TKey, TContext>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly ILogger<PatchEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext>> logger;
        private readonly IEntityRepository<TContext> repository;
        private readonly IEntityBuilder<TEntity, TKey> entityBuilder;

        public PatchEntityFromModelCommandHandler(
            ILogger<PatchEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext>> logger,
            IEntityRepository<TContext> repository,
            IEntityBuilder<TEntity, TKey> entityBuilder)
        {
            this.logger = logger;
            this.repository = repository;
            this.entityBuilder = entityBuilder;
        }
        public override async Task<Result<TKey>> HandleAsync(PatchEntityFromModelCommand<TModel, TEntity, TKey, TContext> request, CancellationToken cancellationToken = default)
        {
            var idName = nameof(IObject<TKey>.Id);
            var delta = request.Delta;
            if (delta.GetChangedPropertyNames().Contains(idName))
            {
                if (delta.TryGetPropertyValue(idName, out var value))
                {
                    if (value is TKey id)
                    {
                        var exists = await this.repository.Where(new EntityByIdSpecification<TEntity, TKey>(id)).AnyAsync(cancellationToken);
                        if (exists)
                        {
                            var entity = this.entityBuilder.Patch(request.Delta);
                            await this.repository.CommitAsync(cancellationToken);
                            return Result<TKey>.Success(id);
                        }
                        else
                        {
                            var error = $"The entity of type {typeof(TEntity).Name} with key {id} does not exits and can not be patched";
                            this.logger.LogDebug(error);
                            return Result<TKey>.Fail(error);
                        }
                    }
                    else
                    {
                        var error = $"The type of {value.GetType()} does not match the key type {typeof(TKey).Name} fot the entity type {typeof(TEntity).Name} and can not be patched";
                        this.logger.LogDebug(error);
                        return Result<TKey>.Fail(error);
                    }
                }
                else
                {
                    var error = $"The delta of entity of type {typeof(TEntity).Name} does not have a key ({idName}) and can not be patched";
                    this.logger.LogDebug(error);
                    return Result<TKey>.Fail(error);
                }
            }
            else
            {
                var error = $"The delta of entity of type {typeof(TEntity).Name} does not have a key property provided ({idName}) and can not be patched";
                this.logger.LogDebug(error);
                return Result<TKey>.Fail(error);
            }
        }
    }
}
