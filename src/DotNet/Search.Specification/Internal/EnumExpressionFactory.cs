using System;
using System.Linq.Expressions;
using Stize.DotNet.Search.Filter;

namespace Stize.DotNet.Search.Specification.Internal
{
    internal static class EnumExpressionFactory
    {
        public static Expression Create(FilterOperator filterOperator, Expression left, ConstantExpression right, Type propertyType)
        {
            var enumType = Enum.GetUnderlyingType(propertyType);
            var convertExpression = Expression.Convert(left, enumType);
            var convertValue = Expression.Convert(right, enumType);

            switch (filterOperator)
            {
                case FilterOperator.IsEqualTo:
                    return Expression.Equal(left, right);
                case FilterOperator.IsNotEqualTo:
                    return Expression.NotEqual(left, right);
                case FilterOperator.IsGreaterThan:
                    return Expression.GreaterThan(convertExpression, convertValue);
                case FilterOperator.IsLessThan:
                    return Expression.LessThan(convertExpression, convertValue);
                case FilterOperator.IsGreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(convertExpression, convertValue);
                case FilterOperator.IsLessThanOrEqualTo:
                    return Expression.LessThanOrEqual(convertExpression, convertValue);
                case FilterOperator.IsContainedIn:
                    return EnumerableExpressionFactory.IsContainedInExpression(left, right, propertyType);
                case FilterOperator.IsNotContainedIn:
                    var isContainedIn = EnumerableExpressionFactory.IsContainedInExpression(left, right, propertyType);
                    return Expression.Not(isContainedIn);
                //Comparisons can't be applied to Enums
                case FilterOperator.Contains:
                case FilterOperator.DoesNotContain:
                case FilterOperator.StartsWith:
                case FilterOperator.EndsWith:
                    return null;
                default:
                    return null;
            }
        }
    }
}