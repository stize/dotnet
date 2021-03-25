using Microsoft.EntityFrameworkCore;
using Stize.CQRS.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Materializer;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public class CreateEntityFromModelCommandHandler<TModel, TEntity, TKey, TContext> : ICommandHandler<CreateEntityFromModelCommand<TModel, TEntity, TKey>, Result<TKey>>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly IEntityRepository<TContext> repository;
        private readonly IEntityBuilder<TModel, TEntity, TKey> entityBuilder;

        public CreateEntityFromModelCommandHandler(IEntityRepository<TContext> repository, IEntityBuilder<TModel, TEntity, TKey> entityBuilder)
        {
            this.repository = repository;
            this.entityBuilder = entityBuilder;
        }
        public async Task<Result<TKey>> HandleAsync(CreateEntityFromModelCommand<TModel, TEntity, TKey> request, CancellationToken cancellationToken = default)
        {           
            var entity = this.entityBuilder.Create(request.Model);
            
            this.repository.Add(entity);
            await this.repository.CommitAsync(cancellationToken);
            
            var result = Result<TKey>.Success(entity.Id);
            return result;
        }
    }
}
