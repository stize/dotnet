using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Stize.DotNet.Search.Specification.Internal
{
    internal static class EnumerableExpressionFactory
    {
        public static Expression IsContainedInExpression(Expression left, Expression right, Type propertyType)
        {
            //var r = new[] {"1", "2", "3"};
            //var l = "1";
            //Enumerable.Contains<string>(r, l);
            // r.Contains(l);

            var method = EnumerableExtensions.ContainsMethod.MakeGenericMethod(propertyType);
            IEnumerable<Expression> parameters = new[]
            {
                right, left
            };
            var exp = Expression.Call(null, method, parameters);

            return exp;
        }
    }
}