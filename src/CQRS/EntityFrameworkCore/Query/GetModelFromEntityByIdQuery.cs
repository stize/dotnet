using Microsoft.EntityFrameworkCore;
using Stize.CQRS.Query;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.DotNet.Specification;
using Stize.Persistence.EntityFrameworkCore;
using Stize.Persistence.Inquiry;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetModelFromEntityByIdQuery<TModel, TEntity, TKey> : IQuery<SingleValueResult<TModel>>
        where TModel : class
        where TEntity : class
    {        
        public GetModelFromEntityByIdQuery(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }

    public class GetModelFromEntityByIdQueryHandler<TModel, TEntity, TKey, TContext> : IQueryHandler<GetModelFromEntityByIdQuery<TModel, TEntity, TKey>, SingleValueResult<TModel>>
        where TModel : class        
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        private readonly IEntityRepository<TContext> repository;

        public GetModelFromEntityByIdQueryHandler(IEntityRepository<TContext> repository)
        {
            this.repository = repository;
        }
        public async Task<SingleValueResult<TModel>> HandleAsync(GetModelFromEntityByIdQuery<TModel, TEntity, TKey> request, CancellationToken cancellationToken = default)
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

    public class EntityByIdSpecification<TEntity, TKey> : Specification<TEntity>
        where TEntity : IEntity<TKey>
    {
        public EntityByIdSpecification(TKey id) : base (ExpressionExtensions.Equals<TEntity, TKey>(ExpressionExtensions.GetPropertyName<TEntity, TKey>(e => e.Id), id))
        {
        }
    }
}
