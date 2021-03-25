using Stize.CQRS.Query;
using Stize.DotNet.Result;
using Stize.DotNet.Search.Page;

namespace Stize.CQRS.EntityFrameworkCore.Query
{
    public class GetAllModelsFromEntityByPageQuery<TModel, TEntity> : IQuery<PagedValueResult<TModel>>
    {
        public GetAllModelsFromEntityByPageQuery(IPageDescriptor pageDescriptor)
        {
            PageDescriptor = pageDescriptor;
        }

        public IPageDescriptor PageDescriptor { get; }
    }
}
