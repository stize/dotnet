using System;
using System.Linq.Expressions;
using Stize.DotNet.Search.Filter;

namespace Stize.DotNet.Search.Specification.Internal
{
    internal static class BooleanExpressionFactory
    {
        public static Expression Create(FilterOperator filterOperator, Expression left, Expression right, Type propertyType)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsEqualTo:
                    return Expression.Equal(left, right);
                case FilterOperator.IsNotEqualTo:
                    return Expression.NotEqual(left, right);
                case FilterOperator.IsContainedIn:
                    return EnumerableExpressionFactory.IsContainedInExpression(left, right, propertyType);
                case FilterOperator.IsNotContainedIn:
                    var isContainedIn = EnumerableExpressionFactory.IsContainedInExpression(left, right, propertyType);
                    return Expression.Not(isContainedIn);
                //Comparisons can't be applied to boolean
                case FilterOperator.Contains:
                case FilterOperator.DoesNotContain:
                case FilterOperator.IsGreaterThan:
                case FilterOperator.IsLessThan:
                case FilterOperator.IsGreaterThanOrEqualTo:
                case FilterOperator.IsLessThanOrEqualTo:
                case FilterOperator.StartsWith:
                case FilterOperator.EndsWith:
                    return null;
                default:
                    return null;
            }
        }
    }
}