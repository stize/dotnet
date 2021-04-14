using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Command;
using Stize.Domain.Entity;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetModelFromEntityByIdQuery<TModel, TEntity, TKey, TContext> :
        EntityQuery<SingleValueResult<TModel>, TModel, TEntity, TKey, TContext>
        where TModel : class
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {        
        public GetModelFromEntityByIdQuery(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}
