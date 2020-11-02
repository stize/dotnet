using System;
using System.Linq.Expressions;

namespace Stize.DotNet.Specification
{
    /// <summary>
    ///     http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    public class Specification<T> : ISpecification<T>
    {
        public static ISpecification<T> False { get; } = new Specification<T>(x => false);
        public static ISpecification<T> True { get; } = new Specification<T>(x => true);

        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.Predicate = predicate;
        }

        public Expression<Func<T, bool>> Predicate { get; }


    }
}