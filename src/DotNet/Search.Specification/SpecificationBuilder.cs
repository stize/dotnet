using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Stize.DotNet.Search.Filter;
using Stize.DotNet.Search.Specification.Internal;
using Stize.DotNet.Specification;
using Stize.DotNet.Specification.Extensions;

namespace Stize.DotNet.Search.Specification
{
    public class SpecificationBuilder : ISpecificationBuilder
    {
        private static readonly MethodInfo CreateExpressionMethodInfo = typeof(SpecificationBuilder).GetMethod(nameof(CreateExpression), BindingFlags.Instance | BindingFlags.NonPublic);


        public const char AllCharOperator = '&';
        public const char AnyCharOperator = '|';

        private readonly char[] CompositeCharOperators = { AllCharOperator, AnyCharOperator };

        private readonly IDictionary<char, MethodInfo> CompositeCharMethods = new Dictionary<char, MethodInfo>()
        {
            {AllCharOperator, EnumerableExtensions.AllMethod},
            {AnyCharOperator, EnumerableExtensions.AnyMethod}
        };

        private readonly ILogger<SpecificationBuilder> logger;

        public SpecificationBuilder(ILogger<SpecificationBuilder> logger)
        {
            this.logger = logger;
        }

        public ISpecification<T> Create<T>(IEnumerable<FilterDescriptor> filters)
        {
            var spec = Specification<T>.True;
            if (filters == null)
            {
                return spec;
            }

            foreach (var filter in filters)
            {
                try
                {
                    var fSpec = this.Create<T>(filter);
                    if (fSpec != null)
                    {
                        spec = spec.And(fSpec);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogInformation(
                        ex,
                        $"There was an error generating the filter for {typeof(T)} - {filter.Member} - {filter.Operator} - {filter.Value}");
                }
            }

            return spec;
        }

        public Specification<T> Create<T>(FilterDescriptor filter)
        {
            var expression = this.CreateExpression<T>(filter);


            if (expression == null)
            {
                return null;
            }

            var spec = new Specification<T>(expression);
            return spec;
        }

        private Expression<Func<T, bool>> CreateExpression<T>(FilterDescriptor filter)
        {
            var index = filter.Member.IndexOfAny(this.CompositeCharOperators);
            var expression = index > -1 ? 
                                this.CreateMemberExpressionComposite<T>(filter, index) : 
                                this.CreateMemberExpressionSimple<T>(filter);

            return expression;
        }


        private Expression<Func<T, bool>> CreateMemberExpressionComposite<T>(FilterDescriptor filter, int index)
        {
            var collectionPath = filter.Member.Substring(0, index);
            var separator = filter.Member[index];
            var childPropertyPath = filter.Member.Substring(index + 1, (filter.Member.Length - (index + 1)));
            var childFilter = new FilterDescriptor
            {
                Member = childPropertyPath,
                Operator = filter.Operator,
                Value = filter.Value
            };

            var (memberAccessParameter, _, collectionPropertyTye) = ExpressionExtensions.MakePropertyExpression<T>(collectionPath);
            var collectionChildPropertyType = collectionPropertyTye.GenericTypeArguments[0];

            var createMethod = CreateExpressionMethodInfo.MakeGenericMethod(collectionChildPropertyType);
            var childExpression = createMethod.Invoke(this, new object[] { childFilter }) as Expression;

            if (childExpression == null) return null;

            var method = this.CompositeCharMethods[separator];
            var concreteMethod = method.MakeGenericMethod(collectionChildPropertyType);

            IEnumerable<Expression> parameters = new[]
            {
                memberAccessParameter, childExpression
            };
            var exp = Expression.Call(null, concreteMethod, parameters);
            var expression = Expression.Lambda<Func<T, bool>>(exp, memberAccessParameter);

            return expression;
        }

        private Expression<Func<T, bool>> CreateMemberExpressionSimple<T>(FilterDescriptor filter)
        {
            var (memberAccessParameter, _, targetPropertyType) = ExpressionExtensions.MakePropertyExpression<T>(filter.Member);
            
            var right = this.GetExpressionValue(filter, targetPropertyType);
            
            var assign = this.GetAssignExpression(targetPropertyType, filter.Operator, memberAccessParameter, right);
            if (assign == null)
            {
                return null;
            }
            return Expression.Lambda<Func<T, bool>>(assign, memberAccessParameter);
        }


        private ConstantExpression GetExpressionValue(FilterDescriptor filter, Type properType)
        {
            switch (filter.Operator)
            {
                case FilterOperator.IsLessThan:
                case FilterOperator.IsLessThanOrEqualTo:
                case FilterOperator.IsEqualTo:
                case FilterOperator.IsNotEqualTo:
                case FilterOperator.IsGreaterThan:
                case FilterOperator.IsGreaterThanOrEqualTo:
                case FilterOperator.StartsWith:
                case FilterOperator.EndsWith:
                case FilterOperator.Contains:
                case FilterOperator.DoesNotContain:
                    var effectiveValue = filter.Value.ConvertTo(properType);
                    return Expression.Constant(effectiveValue, properType);
                case FilterOperator.IsContainedIn:
                case FilterOperator.IsNotContainedIn:
                    var array = filter.Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var arrayValues = array.Select(x => x.ConvertTo(properType));
                    var castMethod = EnumerableExtensions.CastMethod;
                    var genericCastMethod = castMethod.MakeGenericMethod(properType);
                    var obj = genericCastMethod.Invoke(null, new object[] { arrayValues });
                    return Expression.Constant(obj);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Expression GetAssignExpression(Type propertyType, FilterOperator filterOperator, Expression left, ConstantExpression right)
        {
            var effectiveType = propertyType.GetEffectiveType();

            if (effectiveType == typeof(string))
            {
                return StringExpressionFactory.Create(filterOperator, left, right, propertyType);
            }

            if (effectiveType.IsNumericType())
            {
                return NumericExpressionFactory.Create(filterOperator, left, right, propertyType);
            }

            if (effectiveType == typeof(bool))
            {
                return BooleanExpressionFactory.Create(filterOperator, left, right, propertyType);
            }

            if (effectiveType == typeof(DateTime))
            {
                return DateTimeExpressionFactory.Create(filterOperator, left, right, propertyType);
            }

            if (effectiveType.IsEnum())
            {
                return EnumExpressionFactory.Create(filterOperator, left, right, propertyType);
            }

            return null;
        }

    }
}