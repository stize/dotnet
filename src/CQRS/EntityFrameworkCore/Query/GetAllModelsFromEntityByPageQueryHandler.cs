using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Specification;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityByPageQueryHandler<TModel, TEntity, TKey, TContext> :
        EntityQueryHandler<GetAllModelsFromEntityByPageQuery<TModel, TEntity, TKey, TContext>, PagedValueResult<TModel>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly IEntityRepository<TContext> repository;
        private readonly ISpecificationBuilder specificationBuilder;

        public GetAllModelsFromEntityByPageQueryHandler(IEntityRepository<TContext> repository, ISpecificationBuilder specificationBuilder)
        {
            this.repository = repository;
            this.specificationBuilder = specificationBuilder;
        }


        public override async Task<PagedValueResult<TModel>> HandleAsync(GetAllModelsFromEntityByPageQuery<TModel, TEntity, TKey, TContext> request, CancellationToken cancellationToken = default)
        {
            var targetSpecification = this.specificationBuilder.Create<TModel>(request.PageDescriptor.Filters);

            var query = new PagedValueInquiry<TEntity, TModel>()
            {
                Take = request.PageDescriptor.Take,
                Skip = request.PageDescriptor.Skip,
                TargetSorts = request.PageDescriptor.Sorts,
                TargetSpecification = targetSpecification
            };

            var result = await this.repository.RunQueryAsync(query, cancellationToken);

            return result;
        }
    }
}
