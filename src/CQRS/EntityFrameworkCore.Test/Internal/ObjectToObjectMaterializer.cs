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

    public class ObjectToObjectBuilder<TEntity, TKey> : IEntityBuilder<TEntity, TKey>
         where TEntity : class, IEntity<TKey>, new()
    {
        public TEntity Create<TModel>(TModel model) where TModel : class

        {
            return new TEntity();
        }

        public TEntity Patch<TModel>(Delta<TModel> model) where TModel : class
        {
            return new TEntity();
        }

        public TEntity Update<TModel>(TModel model) where TModel : class
        {
            return new TEntity();
        }
    }
}
