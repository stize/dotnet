using System;
using System.Linq;
using System.Threading.Tasks;
using Stize.Domain.Entity;
using Stize.DotNet.Delta;
using Stize.Persistence.Materializer;

namespace Stize.CQRS.EntityFrameworkCore.Test.Internal
{
    public class ObjectToObjectMaterializer<TSource, TTarget> : IMaterializer<TSource, TTarget>
        where TTarget : new()
    {
        public IQueryable<TTarget> Materialize(IQueryable<TSource> Inquiryable)
        {
            return Inquiryable.Select(x => new TTarget());
        }
    }

    public class ObjectToObjectBuilder<TEntity, TKey> : IEntityBuilder<TEntity, TKey>
         where TEntity : class, IEntity<TKey>, new()
    {
        public Task<TEntity> CreateAsync<TModel>(TModel model) where TModel : class

        {
            return Task.FromResult(new TEntity());
        }

        public Task<TEntity> PatchAsync<TModel>(Delta<TModel> model) where TModel : class
        {
            return Task.FromResult(new TEntity());
        }

        public Task<TEntity> UpdateAsync<TModel>(TModel model) where TModel : class
        {
            return Task.FromResult(new TEntity());
        }
    }
}
