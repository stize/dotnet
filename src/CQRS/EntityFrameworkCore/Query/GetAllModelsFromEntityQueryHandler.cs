using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityQueryHandler<TModel, TEntity, TKey, TContext> :
        EntityQueryHandler<GetAllModelsFromEntityQuery<TModel, TEntity, TKey, TContext>, MultipleValueResult<TModel>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly IEntityRepository<TContext> repository;

        public GetAllModelsFromEntityQueryHandler(IEntityRepository<TContext> repository)
        {
            this.repository = repository;
        }

        public override async Task<MultipleValueResult<TModel>> HandleAsync(GetAllModelsFromEntityQuery<TModel, TEntity, TKey, TContext> request, CancellationToken cancellationToken)
        {            
            var query = new MultipleValueInquiry<TEntity, TModel>();
            var result = await this.repository.RunQueryAsync(query, cancellationToken);
            return result;
        }
    }

}
