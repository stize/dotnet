using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public class MultipleValueQuery<T> : Query<T>, IMultipleValueQuery<T>
        where T : class
    {
        public MultipleValueQuery(ISpecification<T> specification) : base(specification)
        {
        }
    }
}