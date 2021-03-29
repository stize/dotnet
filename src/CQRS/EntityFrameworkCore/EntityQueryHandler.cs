using Microsoft.EntityFrameworkCore;
using Stize.CQRS.EntityFrameworkCore.Internal;
using Stize.CQRS.Query;
using Stize.Domain.Entity;
using Stize.DotNet.Result;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.EntityFrameworkCore.Command
{
    public abstract class EntityQueryHandler<TQuery, TResult, TModel, TEntity, TKey, TContext> :
       IQueryHandler<TQuery, TResult>,
       IEntityCqrsRequestHandler<TModel, TEntity, TKey, TContext>
       where TQuery : IQuery<TResult>
       where TResult : IValueResult
       where TModel : class
       where TEntity : class, IEntity<TKey>
       where TContext : DbContext
    {
        public abstract Task<TResult> HandleAsync(TQuery request, CancellationToken cancellationToken = default);
        
    }
}
