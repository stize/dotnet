using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public interface IQuery<T>
        where T : class
    {
        ISpecification<T> Specification { get; }
    }
}