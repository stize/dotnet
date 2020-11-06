using System.Linq;

namespace Stize.Persistence.Materializer
{
    public interface IEntityMaterializer<TEntity, TResult>
    {
        IQueryable<TResult> Materialize(IQueryable<TEntity> queryable);
    }
}
