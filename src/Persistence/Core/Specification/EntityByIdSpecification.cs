using Stize.Domain;
using Stize.DotNet.Specification;
using System.Linq.Expressions;

namespace Stize.Persistence.Specification
{
    public class EntityByIdSpecification<TEntity, TKey> : Specification<TEntity>
        where TEntity : IObject<TKey>
    {
        public EntityByIdSpecification(TKey id) : base(ExpressionExtensions.Equals<TEntity, TKey>(ExpressionExtensions.GetPropertyName<TEntity, TKey>(e => e.Id), id))
        {
        }
    }
}
