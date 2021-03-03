using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.Query.Domain
{
    public class GetModelFromEntityByIdQuery<TModel, TEntity, TKey> : IQuery<SingleValueQueryResult<TModel>>
    {
        public GetModelFromEntityByIdQuery(TKey key)
        {
            this.Key = key;
        }

        public TKey Key { get; }
    }

    public class GetModelFromEntityByIdQueryHandler<TModel, TEntity, TKey> : IQueryHandler<GetModelFromEntityByIdQuery<TModel, TEntity, TKey>, SingleValueQueryResult<TModel>>
    {
        public Task<SingleValueQueryResult<TModel>> HandleAsync(GetModelFromEntityByIdQuery<TModel, TEntity, TKey> request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
