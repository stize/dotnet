using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stize.CQRS.Query.Domain
{
    public class GetAllModelsFromEntityByPageQuery<TModel, TEntity> : IQuery<PagedValueQueryResult<TModel>>
    {
    }

    public class GetAllModelsFromEntityByPageQuery<TModel, TEntity, TKey> : IQueryHandler<GetAllModelsFromEntityByPageQuery<TModel, TEntity>, PagedValueQueryResult<TModel>>
    {

        public Task<PagedValueQueryResult<TModel>> HandleAsync(GetAllModelsFromEntityQuery<TModel, TEntity> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
