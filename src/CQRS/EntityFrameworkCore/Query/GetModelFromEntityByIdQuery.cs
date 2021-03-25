using Stize.CQRS.Query;
using Stize.DotNet.Result;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetModelFromEntityByIdQuery<TModel, TEntity, TKey> : IQuery<SingleValueResult<TModel>>
        where TModel : class
        where TEntity : class
    {        
        public GetModelFromEntityByIdQuery(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}
