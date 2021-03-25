using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stize.Domain;
using Stize.DotNet.Delta;
using Stize.DotNet.Result;
using Stize.DotNet.Specification;

namespace Stize.Mapping.Service
{
    public interface IMappingService<TContext>
    {
        Task<IEnumerable<TModel>> GetAllAsync<TModel, TEntity>(CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class;

        
        Task RemoveAsync<TModel, TEntity>(TModel model, CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class;

        Task<IEnumerable<TModel>> WhereAsync<TModel, TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class;

        Task<bool> AnyAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            //where TModel : class 
            where TEntity : class;

        Task<int> CountAsync<TEntity>(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
            //where TModel : class 
            where TEntity : class;

        Task<TKey> AddAsync<TModel, TEntity, TKey>(TModel model, CancellationToken cancellationToken = default)
            where TModel : class
            where TEntity : class, IObject<TKey>;

        Task<TModel> FindOneAsync<TModel, TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
            where TModel : class, IObject<TKey>
            where TEntity : class, IObject<TKey>;

        Task<Result<TKey>> RemoveAsync<TEntity, TKey>(TKey id, CancellationToken cancellationToken = default)
            where TEntity : class, IObject<TKey>;

        Task<Result<TKey>> UpdateAsync<TModel, TEntity, TKey>(TModel model, CancellationToken cancellationToken = default)
            where TModel : class, IObject<TKey>
            where TEntity : class, IObject<TKey>;

        Task<Result<TKey>> PatchAsync<TModel, TEntity, TKey>(Delta<TModel> delta, CancellationToken cancellationToken = default)
            where TModel : class, IObject<TKey>
            where TEntity : class, IObject<TKey>;
    }
}