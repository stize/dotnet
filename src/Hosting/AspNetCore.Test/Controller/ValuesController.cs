using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stize.Domain.Model;
using Stize.DotNet.Result;
using Stize.Hosting.AspNetCore.Extensions;
using Stize.Hosting.AspNetCore.Test.Model;
using Stize.Persistence.QueryResult;

namespace Stize.Hosting.AspNetCore.Test.Controller
{

    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : Microsoft.AspNetCore.Mvc.Controller
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // GET api/values/5
        [HttpGet("paginated")]
        public IActionResult GetPaginated()
        {
            var page = PaginatedResult<object>.Ok(new object[] {1, 2, 3}).WithTotal(3);
            
            return this.PagedJsonResult(page, true);
        }

        // GET api/values/5
        [HttpPost("multiuploadmodel_model")]
        public IActionResult GetPaginatedModel([FromBody] MultiUploadedFileModel<TestModel> model)
        {
            return this.Json(model.Model);
        }

        // GET api/values/5
        [HttpPost("multiuploadmodel_file")]
        public async Task<IActionResult> GetPaginatedFile([FromBody] MultiUploadedFileModel<TestModel> model)
        {
            var file = model.Files.First();
            var buffer = new byte[file.ContentLength];
            var bytesRead = await file.FileStream.ReadAsync(buffer, 0, buffer.Length);
            return this.FileResult(buffer, file.OriginalName);
        }

    }
}
