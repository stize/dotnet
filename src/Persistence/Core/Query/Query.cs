using System;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public class Query<T> : IQuery<T>
        where T : class
    {
        public Query(ISpecification<T> specification)
        {
            this.Specification = specification;
        }

        public ISpecification<T> Specification { get; }

    }
}