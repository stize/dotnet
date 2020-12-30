using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Stize.Hosting.AspNetCore.ActionResult;
using Stize.Persistence.QueryResult;


namespace Stize.Hosting.AspNetCore.Extensions
{
    public static class WebApiControllerExtensions
    {
        public static IActionResult ValidationErrorResult(this ControllerBase controller)
        {
            var errors = from s in controller.ModelState
                from e in s.Value.Errors
                where s.Value.Errors.Any()
                select new {s.Key, e.ErrorMessage};

            return new JsonResult(errors);
        }

        public static IActionResult PagedJsonResult<T>(this ControllerBase controller, IPagedQueryResult<T> result, bool envelope = false)
            where T : class
        {
            return new PagedJsonResult<T>(result, envelope);
        }

        public static IActionResult FileResult(this ControllerBase controller, byte[] fileBytes, string fileName)
        {
            return new ActionResult.FileResult(fileBytes, fileName);
        }
    }
}