using Stize.Domain.Entity;
using Stize.DotNet.Delta;
using System.Threading.Tasks;

namespace Stize.Persistence.Materializer
{
    public interface IEntityBuilder<TEntity, TKey>        
        where TEntity : class, IEntity<TKey>
    {
        Task<TEntity> CreateAsync<TModel>(TModel model) where TModel : class;
        Task<TEntity> UpdateAsync<TModel>(TModel model) where TModel : class;
        Task<TEntity> PatchAsync<TModel>(Delta<TModel> model) where TModel : class;
    }
}
