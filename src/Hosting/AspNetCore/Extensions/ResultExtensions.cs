using Microsoft.AspNetCore.Mvc;
using Stize.DotNet.Result;
using Stize.DotNet.Result.Reasons;

namespace Stize.Hosting.AspNetCore.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this WrappedResult<T> result, bool envelope = false)
        {
            if (!result.Success)
            {
                var problemDetails = new ProblemDetails();
                problemDetails.Extensions.Add(nameof(Reason), result.Reasons);

                return new BadRequestObjectResult(problemDetails);
            }

            if (!envelope)
            {
                return new OkObjectResult(result.Value);
            }

            return new OkObjectResult(result);
        }
    }
}