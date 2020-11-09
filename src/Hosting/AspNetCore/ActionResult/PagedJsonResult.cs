using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Stize.DotNet.Json;
using Stize.Hosting.AspNetCore.Model;
using Stize.Persistence.QueryResult;

namespace Stize.Hosting.AspNetCore.ActionResult
{
    public abstract class PagedJsonResult
    {
        public const string PaginationTotal = "X-Pagination-Total";
        public const string PaginationTake = "X-Pagination-Take";
        public const string PaginationSkip = "X-Pagination-Skip";
        public const string PaginationLink = "Link";
    }

    public class PagedJsonResult<T> : PagedJsonResult, IActionResult
        where T : class
    {
        private readonly IPagedQueryResult<T> content;
        private readonly Controller controller;
        private readonly bool envelope;
        private readonly IJsonSerializer serializer;

        public PagedJsonResult(IPagedQueryResult<T> content, bool envelope, Controller controller)
        {
            this.content = content;
            this.envelope = envelope;
            this.controller = controller;
            this.serializer = controller.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var headers = response.GetTypedHeaders();

            response.StatusCode = (int)HttpStatusCode.OK;
            headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                Charset = Encoding.UTF8.WebName
            };
            this.AddPaginationHeaders(headers);

            var arraySegment = await this.SerializeAsync();
            using (var byteArray = new ByteArrayContent(arraySegment.Array, arraySegment.Offset, arraySegment.Count))
            {
                var buffer = await byteArray.ReadAsByteArrayAsync();
                await response.BodyWriter.WriteAsync(buffer);
            }

        }

        public virtual void AddPaginationHeaders(ResponseHeaders headers)
        {
            var first = 0;
            var prev = this.content.Skip - this.content.Take;
            var next = this.content.Skip + this.content.Take;
            var last = this.content.Total - this.content.Take;

            var uri = this.controller.Request.GetDisplayUrl();


            var firstString = $"<{uri}?skip={first}&take={this.content.Take}>; rel=\"first\"";
            var prevString = $"<{uri}?skip={prev}&take={this.content.Take}>; rel=\"prev\"";
            var nextString = $"<{uri}?skip={next}&take={this.content.Take}>; rel=\"next\"";
            var lastString = $"<{uri}?skip={last}&take={this.content.Take}>; rel=\"last\"";

            var linkHeader = $"Link: {firstString}, {prevString}, {nextString}, {lastString}";

            headers.Append(PaginationLink, linkHeader);


            headers.Append(PaginationTotal, this.content.Total.ToString());
            if (this.content.Take.HasValue)
            {
                headers.Append(PaginationTake, this.content.Take.ToString());
            }

            if (this.content.Skip.HasValue)
            {
                headers.Append(PaginationSkip, this.content.Skip.ToString());
            }
        }

        private async Task<ArraySegment<byte>> SerializeAsync()
        {

            var value = this.GetContent();
            var json = await this.serializer.SerializeAsync(value);
            var bytes = Encoding.UTF8.GetBytes(json);
            return new ArraySegment<byte>(bytes, 0, bytes.Length);

        }

        public virtual object GetContent()
        {
            return !this.envelope
                ? (object)this.content.Result
                : new EnvelopedModel<T>
                {
                    Take = this.content.Take,
                    Skip = this.content.Skip,
                    Total = this.content.Total,
                    Data = this.content.Result
                };
        }
    }
}