using System;
using System.Linq.Expressions;

namespace Stize.DotNet.Specification
{
   
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        protected readonly ISpecification<T> LeftSide;

        protected readonly ISpecification<T> RightSide;

        protected CompositeSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            this.LeftSide = leftSide;
            this.RightSide = rightSide;
        }

        public abstract Expression<Func<T, bool>> Predicate { get; }

        
    }
}