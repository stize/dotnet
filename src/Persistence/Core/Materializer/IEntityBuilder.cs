using Stize.Domain.Entity;
using Stize.DotNet.Delta;

namespace Stize.Persistence.Materializer
{
    public interface IEntityBuilder<TEntity, TKey>        
        where TEntity : class, IEntity<TKey>
    {
        TEntity Create<TModel>(TModel model) where TModel : class;
        TEntity Update<TModel>(TModel model) where TModel : class;
        TEntity Patch<TModel>(Delta<TModel> model) where TModel : class;
    }
}
