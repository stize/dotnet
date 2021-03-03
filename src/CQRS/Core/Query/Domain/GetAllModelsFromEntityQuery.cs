using Stize.CQRS.Mediator;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.CQRS.Query.Domain
{
    public class GetAllModelsFromEntityQuery<TModel, TEntity> : IQuery<MultipleValueQueryResult<TModel>>
    {
    }

    public class GetAllModelsFromEntityQueryHandler<TModel, TEntity, TKey> : IQueryHandler<GetAllModelsFromEntityQuery<TModel, TEntity>, MultipleValueQueryResult<TModel>>
    {
        
        public Task<MultipleValueQueryResult<TModel>> HandleAsync(GetAllModelsFromEntityQuery<TModel, TEntity> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
