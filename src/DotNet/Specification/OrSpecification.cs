using System;
using System.Linq.Expressions;

namespace Stize.DotNet.Specification
{
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
            : base(leftSide, rightSide)
        {
        }

        public override Expression<Func<T, bool>> Predicate => this.LeftSide.Predicate.OrElse(this.RightSide.Predicate);

        
    }
}