using System.Linq;

namespace Stize.Persistence.Materializer
{
    public interface IMaterializer<in TSource, out TTarget>
    {
        IQueryable<TTarget> Materialize(IQueryable<TSource> queryable);
    }
}