using System;
using System.Linq.Expressions;

namespace Stize.DotNet.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

    }
}