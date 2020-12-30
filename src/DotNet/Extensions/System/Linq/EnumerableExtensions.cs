using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static readonly MethodInfo ContainsMethod = ExpressionExtensions.GetMethodInfo(typeof(Enumerable), "Contains", 2);
        public static readonly MethodInfo CastMethod = ExpressionExtensions.GetMethodInfo(typeof(Enumerable), "Cast", 1);

        public static readonly MethodInfo AllMethod = ExpressionExtensions.GetMethodInfo(typeof(Enumerable), "All", 2);
        public static readonly MethodInfo AnyMethod = ExpressionExtensions.GetMethodInfo(typeof(Enumerable), "Any", 2);

    }
}