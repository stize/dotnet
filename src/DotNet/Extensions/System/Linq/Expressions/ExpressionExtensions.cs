using System.Reflection;
using Stize.DotNet.Extensions.Common;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }


        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> Equals<T, TValue>(string propertyName, TValue constant)
        {
            var type = typeof(T);
            var parameterExpression = Expression.Parameter(type, type.Name);
            return
                Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(Expression.Property(parameterExpression, propertyName),
                        Expression.Constant(constant)), parameterExpression);
        }

        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var unaryExpression = propertyLambda.Body as UnaryExpression;
            var memberExpression = unaryExpression == null
                ? propertyLambda.Body as MemberExpression
                : unaryExpression.Operand as MemberExpression;

            if (memberExpression == null)
            {
                throw Error.Argument(SRResources.LambdaFormatErrorNotParameter, nameof(memberExpression));
            }

            return memberExpression.Member.Name;
        }

        public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            var unaryExpression = propertyLambda.Body as UnaryExpression;
            var memberExpression = unaryExpression == null
                ? propertyLambda.Body as MemberExpression
                : unaryExpression.Operand as MemberExpression;

            if (memberExpression == null)
            {
                throw Error.Argument(SRResources.LambdaFormatErrorNotParameter, nameof(memberExpression));
            }

            return memberExpression.Member.Name;
        }

        public static string GetPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
        {
            var unaryExpression = propertyLambda.Body as UnaryExpression;
            var memberExpression = unaryExpression == null
                ? propertyLambda.Body as MemberExpression
                : unaryExpression.Operand as MemberExpression;

            if (memberExpression == null)
            {
                throw Error.Argument(SRResources.LambdaFormatErrorNotParameter, nameof(memberExpression));
            }

            var memberName = GetMemberExpressionName(memberExpression);

            return memberName;
        }

        public static (Expression propertySelectorExpression, Type targetPropertyType) MakeMemberSelectorExpression<T>(string propertyName)
        {
            Type targetPropertyType = null;

            var propertyRoute = propertyName.Split('.');
            MemberExpression memberAccess = null;
            var targetType = typeof(T);

            ParameterExpression memberAccessParam = Expression.Parameter(targetType);
            foreach (var route in propertyRoute)
            {
                var propertyInfo = targetType.GetProperties()
                                                .FirstOrDefault(p => p.Name.Equals(route, StringComparison.OrdinalIgnoreCase));
                if (propertyInfo == null)
                {
                    throw Error.Argument(Error.Format(SRResources.PropertyMissing, route, targetType), nameof(propertyInfo));
                }

                targetPropertyType = propertyInfo.PropertyType;

                var x = memberAccess ?? (Expression)memberAccessParam;

                var propertyType = propertyInfo.DeclaringType != propertyInfo.ReflectedType ? propertyInfo.DeclaringType : propertyInfo.ReflectedType;
                memberAccess = Expression.Property(x, propertyType, propertyInfo.Name);

                targetType = propertyInfo.PropertyType;
            }

            if (memberAccess == null)
            {
                throw Error.Argument(Error.Format(SRResources.PropertyMissing, propertyName, typeof(T)), nameof(memberAccess));
            }

            // build lambda expression: item => item.fieldName
            var propertySelectorExpression = Expression.Lambda(memberAccess, memberAccessParam);
            return (propertySelectorExpression, targetPropertyType);
        }

        public static (ParameterExpression memberAccessParameter, Expression propertySelectorExpression, Type targetPropertyType) MakePropertyExpression<T>(string propertyName)
        {
            var targetType = typeof(T);
            var memberAccessParameter = Expression.Parameter(targetType);
            Expression propertySelectorExpression = null;
            var propertyRoute = propertyName.Split('.');
            Type targetPropertyType = null;
            foreach (var route in propertyRoute)
            {
                if (route.IsNullOrEmpty())
                {
                    targetPropertyType = targetType;
                    propertySelectorExpression = memberAccessParameter;
                }
                else
                {
                    var type = (targetPropertyType ?? targetType);
                    var propertyInfo = type.GetProperties().FirstOrDefault(p => p.Name.Equals(route, StringComparison.OrdinalIgnoreCase));
                    if (propertyInfo != null)
                    {
                        targetPropertyType = propertyInfo.PropertyType;
                        propertySelectorExpression = Expression.Property(propertySelectorExpression ?? memberAccessParameter, route);
                    }
                    else
                    {
                        throw Error.Argument(Error.Format(SRResources.PropertyMissing, route, targetType), nameof(propertyInfo));
                    }
                }

            }
            return(memberAccessParameter, propertySelectorExpression, targetPropertyType);
        }

        private static string GetMemberExpressionName(MemberExpression expression)
        {
            var memberName = expression.Member.Name;

            if (expression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                memberName = GetMemberExpressionName((MemberExpression)expression.Expression) + "." + memberName;
            }

            return memberName;
        }

        public static MethodInfo GetMethodInfo(Type type, string methodName, int parametersLength)
        {
            return type.GetMethods().First(m => m.Name == methodName && m.GetParameters().Length == parametersLength);
        }

    }
}