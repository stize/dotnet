using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Json;
using Stize.Testing.Xunit.AspNetCore.Internal;

namespace Stize.Testing.Xunit.AspNetCore.TestHost
{
    public static class TestServerExtensions
    {
        public static RequestBuilder CreateApiRequest<TController>(this TestServer server, Expression<Func<TController, object>> actionSelector)
            where TController : class
        {

            var methodExpression = GetMethodExpression(actionSelector);
            var endpoint = GetEndpoint(server, methodExpression);
            var (path, content) = GetInvocationInfo(server, methodExpression, endpoint);

            var requestBuilder = server.CreateRequest(path);
            if (content != null)
            {
                requestBuilder.And(r =>
                {
                    r.Content = content;
                });
            }
            return requestBuilder;
        }

        private static RouteEndpoint GetEndpoint(TestServer server, MethodCallExpression methodExpression)
        {
            var dataSource = server.Services.GetRequiredService<EndpointDataSource>();
            var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().FirstOrDefault(e =>
            {
                var action = e.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();

                var isMethod = action?.MethodInfo == methodExpression.Method;
                return isMethod;
            });

            if (endpoint == null)
            {
                throw new InvalidOperationException("No endpoint found for required action");
            }

            return endpoint;
        }

        private static (string path, HttpContent content) GetInvocationInfo(TestServer server, MethodCallExpression methodExpression, RouteEndpoint endpoint)
        {
            var binderFactory = server.Services.GetRequiredService<TemplateBinderFactory>();
            var binder = binderFactory.Create(endpoint.RoutePattern);
            var routeValues = new RouteValueDictionary();
            var methodParameters = methodExpression.Method.GetParameters();
            HttpContent content = null;
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var parameter = methodParameters[i];
                var value = GetExpressionValue(methodExpression.Arguments[i]);
                if (value != null)
                {
                    if (endpoint.RoutePattern.Parameters.Any(rp => rp.Name == parameter.Name))
                    {

                        routeValues.Add(parameter.Name, value);
                    }
                    else if (parameter.GetCustomAttribute<FromBodyAttribute>() is not null)
                    {
                        var serializer = server.Services.GetRequiredService<IJsonSerializer>();
                        var json = serializer.Serialize(value);
                        content = new StringContent(
                            json,
                            Encoding.UTF8,
                            "application/json");
                    }
                    else if (parameter.GetCustomAttribute<FromFormAttribute>() is not null)
                    {
                        content = new FormUrlEncodedContent(value.ToKeyValue());
                    }
                }


            }
            var path = binder.BindValues(routeValues);

            return (path, content);
        }

        private static object GetExpressionValue(Expression argument)
        {
            if (argument is ConstantExpression constant)
            {
                return constant.Value;
            }

            if (argument is MemberExpression member)
            {
                return Expression.Lambda(member).Compile().DynamicInvoke();
            }

            if (argument is NewExpression create)
            {
                return Expression.Lambda(create).Compile().DynamicInvoke();
            }
            throw new NotSupportedException($"Expression {argument} is not supported");
        }

        private static MethodCallExpression GetMethodExpression<TController>(Expression<Func<TController, object>> actionSelector)
            where TController : class
        {
            if (actionSelector.NodeType == ExpressionType.Lambda && actionSelector.Body is MethodCallExpression bodyExpression)
            {
                return bodyExpression;
            }

            throw new InvalidOperationException($"The action selector is not a valid lambda expression");
        }
    }
}