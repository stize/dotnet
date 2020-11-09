using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace Stize.Hosting.AspNetCore.Filter
{
    public class NoCacheHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Response != null) // can be null when exception happens
            {
                var headers = context.HttpContext.Response.GetTypedHeaders();

                headers.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    MustRevalidate = true
                };

                headers.Headers.Append("pragma", "no-cache");

                if (context.HttpContext.Response.ContentLength.HasValue) // can be null (for example HTTP 400)
                {
                    headers.Expires = DateTimeOffset.UtcNow;
                }
            }
        }
    }
}