using System;
using System.Linq.Expressions;

namespace Stize.DotNet.Specification
{
    
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
            : base(leftSide, rightSide)
        {
        }

        public override Expression<Func<T, bool>> Predicate => this.LeftSide.Predicate.AndAlso(this.RightSide.Predicate);


        
    }
}