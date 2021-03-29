using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityByPageQuery<TModel, TEntity, TKey, TContext> :
        EntityQuery<PagedValueResult<TModel>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        public GetAllModelsFromEntityByPageQuery(IPageDescriptor pageDescriptor)
        {
            PageDescriptor = pageDescriptor;
        }

        public IPageDescriptor PageDescriptor { get; }
    }
}
