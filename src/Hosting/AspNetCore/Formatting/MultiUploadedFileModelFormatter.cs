using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Stize.Domain.File;
using Stize.Domain.Model;
using Stize.DotNet.Json;

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
                 typeof(MultiUploadedFileModel).IsAssignableFrom(context.ModelType))
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

            var model = await this.GetModelAsync(context);
            return await InputFormatterResult.SuccessAsync(model);
        }

        protected virtual async Task<object> GetModelAsync(InputFormatterContext context)
        {
            var fileModel = Activator.CreateInstance(context.ModelType) as IMultiUploadedFile;
            if (fileModel == null) throw new ArgumentException($"The type {context.ModelType.Name} is not assignable to {typeof(IMultiUploadedFile).Name}");
            var files = this.GetStreamFiles(context.HttpContext.Request);
            fileModel.Files = files;

            if (context.ModelType.IsGenericType)
            {
                var model = await this.GetModelAsync(context.ModelType, context.HttpContext.Request);
                var property = fileModel.GetType().GetProperty(nameof(IMultiUploadedFile<object>.Model));
                property.SetValue(fileModel, model );
            }
            return fileModel;
        }

        protected virtual IStreamFileInfo[] GetStreamFiles(HttpRequest request)
        {
            var files = request.Form.Files.Select(f =>
            {
                var contentType = MediaTypeHeaderValue.Parse(f.ContentType);
                var fileContentType = contentType.MediaType;
                return new StreamFileModel(f.OpenReadStream(), f.FileName, fileContentType.Value);
            }).Cast<IStreamFileInfo>().ToArray();
            return files;
        }

        protected virtual async Task<object> GetModelAsync(Type modelType, HttpRequest request)
        {
            object model = null;

            var formData = request.Form.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            if (formData.TryGetValue("model", out var value))
            {
                var json = value;
                var serializer = request.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>();
                var targetType = modelType.GetGenericArguments()[0];
                model = await serializer.DeserializeAsync(json, targetType);
            }

            return model;
        }
    }
}