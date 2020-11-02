using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static readonly MethodInfo OrderByMethod = ExpressionExtensions.GetMethodInfo(typeof(Queryable), "OrderBy", 2);
        public static readonly MethodInfo OrderByDescendingMethod = ExpressionExtensions.GetMethodInfo(typeof(Queryable), "OrderByDescending", 2);
        public static readonly MethodInfo ThenByMethod = ExpressionExtensions.GetMethodInfo(typeof(Queryable), "ThenBy", 2);
        public static readonly MethodInfo ThenByDescendingMethod = ExpressionExtensions.GetMethodInfo(typeof(Queryable), "ThenByDescending", 2);

        public static IOrderedQueryable<T> OrderByPropertyName<T>(this IQueryable<T> dataCollection, MethodInfo orderOperator, string propertyName)
        {
            var keySelectorValue = ExpressionExtensions.MakeMemberSelectorExpression<T>(propertyName);
            // build concrete MethodInfo from the generic one
            orderOperator = orderOperator.MakeGenericMethod(typeof(T), keySelectorValue.Item2);
            // invoke method on dataCollection
            return orderOperator.Invoke(null, new object[] { dataCollection, keySelectorValue.Item1 }) as IOrderedQueryable<T>;
        }
    }
}
