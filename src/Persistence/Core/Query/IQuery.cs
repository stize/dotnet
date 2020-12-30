using System;
using Stize.DotNet.Specification;

namespace Stize.Persistence.Query
{
    public interface IQuery
    {
    }

    public interface IQuery<T> : IQuery
        where T : class
    {
        ISpecification<T> Specification { get; }
    }
}