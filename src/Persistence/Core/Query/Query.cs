using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public abstract class Query<T> : IQuery<T>
        where T : class
    {
        protected Query(ISpecification<T> specification)
        {
            this.Specification = specification;
        }

        public ISpecification<T> Specification { get; }
    }
}