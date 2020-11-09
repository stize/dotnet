using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Stize.Hosting.AspNetCore.Model;

namespace Stize.Hosting.AspNetCore.Formatting
{
    public class MultiUploadedFileModelFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var requestContentType = MediaTypeHeaderValue.Parse(context.HttpContext.Request.ContentType);

            if ((requestContentType.MediaType.Value == "application/octet-stream" ||
                 requestContentType.MediaType.Value == "multipart/form-data") &&
                context.ModelType == this.GetModelType())
            {
                return true;
            }


            return false;
        }

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext.Request;
            
            if (request.ContentLength == 0)
            {
                var contextModelType = context.ModelType;
                var m = contextModelType.GetTypeInfo().IsValueType ? Activator.CreateInstance(contextModelType) : null;
                return await InputFormatterResult.SuccessAsync(m);
            }
            
            var model = await this.GetModelAsync(request);
            return await InputFormatterResult.SuccessAsync(model);
        }

        protected virtual Type GetModelType()
        {
            return typeof(MultiUploadedFileModel);
        }


        protected virtual Task<object> GetModelAsync(HttpRequest request)
        {
            var files = GetStreamFiles(request);

            

            object fileModel = new MultiUploadedFileModel
            {
                Files = files
            };
            return Task.FromResult(fileModel);
        }

        protected StreamFileModel[] GetStreamFiles(HttpRequest request)
        {
            var files = request.Form.Files.Select(f =>
            {
                var contentType = MediaTypeHeaderValue.Parse(f.ContentType);
                var fileContentType = contentType.MediaType;
                return new StreamFileModel(f.OpenReadStream(), f.FileName, fileContentType.Value);
            }).ToArray();
            return files;
        }
    }

    public class MultiUploadedFileModelFormatter<T> : MultiUploadedFileModelFormatter
    {
        protected override Type GetModelType()
        {
            return typeof(MultiUploadedFileModel<T>);
        }

        protected override async Task<object> GetModelAsync(HttpRequest request)
        {
            var model = default(T);

            var formData = request.Form.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            if (formData.TryGetValue("model", out var value))
            {
                var json = value;
                var serializer = request.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
                model = await serializer.DeserializeAsync<T>(json);
            }


            var files = this.GetStreamFiles(request);

            var fileModel = new MultiUploadedFileModel<T>
            {
                Files = files,
                Model = model
            };
            return fileModel;
        }
    }
}