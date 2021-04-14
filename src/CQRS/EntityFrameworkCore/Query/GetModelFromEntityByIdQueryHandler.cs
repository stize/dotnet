using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using Stize.Persistence.Specification;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetModelFromEntityByIdQueryHandler<TModel, TEntity, TKey, TContext> :
        EntityQueryHandler<GetModelFromEntityByIdQuery<TModel, TEntity, TKey, TContext>, SingleValueResult<TModel>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly IEntityRepository<TContext> repository;

        public GetModelFromEntityByIdQueryHandler(IEntityRepository<TContext> repository)
        {
            this.repository = repository;
        }
        public override async Task<SingleValueResult<TModel>> HandleAsync(GetModelFromEntityByIdQuery<TModel, TEntity, TKey, TContext> request, CancellationToken cancellationToken = default)
        {
            var sourceSpecification = new EntityByIdSpecification<TEntity, TKey>(request.Key);
            var query = new SingleValueInquiry<TEntity, TModel>()
            {
                SourceSpecification = sourceSpecification
            };

            var result = await this.repository.RunQueryAsync(query, cancellationToken);
            
            return result;
        }
    }
}
