using Microsoft.EntityFrameworkCore;
using Stize.CQRS.Query;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityQueryHandler<TModel, TEntity, TKey, TContext> : IQueryHandler<GetAllModelsFromEntityQuery<TModel, TEntity>, MultipleValueResult<TModel>>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly IEntityRepository<TContext> repository;

        public GetAllModelsFromEntityQueryHandler(IEntityRepository<TContext> repository)
        {
            this.repository = repository;
        }

        public async Task<MultipleValueResult<TModel>> HandleAsync(GetAllModelsFromEntityQuery<TModel, TEntity> request, CancellationToken cancellationToken)
        {            
            var query = new MultipleValueInquiry<TEntity, TModel>();
            var result = await this.repository.RunQueryAsync(query, cancellationToken);
            return result;
        }
    }

}
