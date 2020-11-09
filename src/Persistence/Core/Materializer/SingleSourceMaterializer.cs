using System.Linq;

namespace Stize.Persistence.Materializer
{
    public class SingleSourceMaterializer<TSource> : IMaterializer<TSource, TSource>
    {
        public IQueryable<TSource> Materialize(IQueryable<TSource> queryable)
        {
            return queryable;
        }
    }
}