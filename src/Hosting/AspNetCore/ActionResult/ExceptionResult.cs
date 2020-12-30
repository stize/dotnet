using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stize.Hosting.AspNetCore.ActionResult
{
    public class ExceptionResult : Microsoft.AspNetCore.Mvc.JsonResult
    {
        public ExceptionResult(Exception exception) : base(exception)
        {
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = StatusCodes.Status500InternalServerError;
            return base.ExecuteResultAsync(context);
        }

    }
}