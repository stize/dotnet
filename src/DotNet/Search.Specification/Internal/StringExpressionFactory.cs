using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Stize.DotNet.Search.Filter;

namespace Stize.DotNet.Search.Specification.Internal
{
    internal static class StringExpressionFactory
    {
        public static Expression Create(FilterOperator filterOperator, Expression left, Expression right, Type propertyType)
        {
            switch (filterOperator)
            {
                case FilterOperator.IsEqualTo:
                    return Expression.Equal(left, right);
                case FilterOperator.IsNotEqualTo:
                    return Expression.NotEqual(left, right);
                case FilterOperator.StartsWith:
                    return StartsWithExpression(left, right);
                case FilterOperator.EndsWith:
                    return EndsWithExpression(left, right);
                case FilterOperator.Contains:
                    return ContainsWithExpression(left, right);
                case FilterOperator.DoesNotContain:
                    var contains = ContainsWithExpression(left, right);
                    return Expression.Not(contains);
                case FilterOperator.IsContainedIn:
                    return EnumerableExpressionFactory.IsContainedInExpression(left, right, propertyType);
                case FilterOperator.IsNotContainedIn:
                    var isContainedIn = EnumerableExpressionFactory.IsContainedInExpression(left, right, propertyType);
                    return Expression.Not(isContainedIn);
                //Comparisons can't be applied to strings
                case FilterOperator.IsGreaterThan:
                case FilterOperator.IsLessThan:
                case FilterOperator.IsGreaterThanOrEqualTo:
                case FilterOperator.IsLessThanOrEqualTo:
                    return null;
                default:
                    return null;
            }
        }

        private static Expression StartsWithExpression(Expression left, Expression right)
        {
            var method = StringExtensions.StartsWithMethod;
            IEnumerable<Expression> parameters = new[]
            {
                right //, Expression.Constant(StringComparison.InvariantCultureIgnoreCase)
            };
            var exp = Expression.Call(left, method, parameters);

            return exp;
        }

        private static Expression EndsWithExpression(Expression left, Expression right)
        {
            var method = StringExtensions.EndsWithMethod;
            IEnumerable<Expression> parameters = new[]
            {
                right //, Expression.Constant(StringComparison.InvariantCultureIgnoreCase)
            };
            var exp = Expression.Call(left, method, parameters);

            return exp;
        }

        private static Expression ContainsWithExpression(Expression left, Expression right)
        {
            var method = StringExtensions.ContainsMethod;
            IEnumerable<Expression> parameters = new[]
            {
                right
            };
            var exp = Expression.Call(left, method, parameters);

            return exp;
        }
    }
}
