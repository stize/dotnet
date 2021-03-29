using System;
using System.Linq;
using Stize.Domain.Entity;
using Stize.DotNet.Delta;
using Stize.Persistence.Materializer;

namespace Stize.CQRS.EntityFrameworkCore.Test.Internal
{
    public class ObjectToObjectMaterializer<TSource, TTarget> : IMaterializer<TSource, TTarget>
        where TTarget: new()
    {
        public IQueryable<TTarget> Materialize(IQueryable<TSource> Inquiryable)
        {
            return Inquiryable.Select(x => new TTarget());
        }
    }

    public class ObjectToObjectBuilder<TModel, TEntity, TKey> : IEntityBuilder<TModel, TEntity, TKey>
        where TModel : class
         where TEntity : class, IEntity<TKey>, new()
    {
        public TEntity Create(TModel model)
        {
            return new TEntity();
        }

        public TEntity Patch(Delta<TModel> model)
        {
            return new TEntity();
        }

        public TEntity Update(TModel model)
        {
            return new TEntity();
        }
    }
}
