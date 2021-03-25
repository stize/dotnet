using Stize.Domain.Entity;
using Stize.DotNet.Delta;

namespace Stize.Persistence.Materializer
{
    public interface IEntityBuilder<TModel, TEntity, TKey>
        where TModel : class
         where TEntity : class, IEntity<TKey>
    {
        TEntity Create(TModel model);
        TEntity Update(TModel model);
        TEntity Patch(Delta<TModel> model);
    }
}
