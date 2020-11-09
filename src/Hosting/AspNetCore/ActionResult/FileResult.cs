using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stize.Hosting.AspNetCore.ActionResult
{
    public class FileResult :Microsoft.AspNetCore.Mvc.ActionResult
    {
        public FileResult(byte[] fileBytes, string fileName)
        {
            this.FileBytes = fileBytes;
            this.FileName = fileName;

        }

        public string FileName { get; }

        public byte[] FileBytes { get; }


        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = MimeTypes.GetMimeType(this.FileName);
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + this.FileName });

            await response.BodyWriter.WriteAsync(this.FileBytes);

        }
    }
}

