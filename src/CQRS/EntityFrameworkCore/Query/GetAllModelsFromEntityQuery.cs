using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityQuery<TModel, TEntity, TKey, TContext> :
        EntityQuery<MultipleValueResult<TModel>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
    }

}
