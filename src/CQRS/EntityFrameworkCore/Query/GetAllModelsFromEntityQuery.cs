using Stize.CQRS.Query;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityQuery<TModel, TEntity> : IQuery<MultipleValueResult<TModel>>
    {
    }

}
