using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Stize.DotNet.Json;
using Stize.Hosting.AspNetCore.Test.Controller;
using Stize.Hosting.AspNetCore.Test.Model;
using Stize.Testing.Xunit.AspNetCore;
using Stize.Testing.Xunit.AspNetCore.Mvc;
using Stize.Testing.Xunit.AspNetCore.TestHost;
using Xunit;
using Xunit.Abstractions;

namespace Stize.Hosting.AspNetCore.Test.MultiUploadedFileModelFormatter
{
    public class MultiUploadedFileModelFormatterBaseTest : WebApplicationTest<Startup>
    {
        private readonly IJsonSerializer serializer;

        public MultiUploadedFileModelFormatterBaseTest(WebApplicationFactory<Startup> fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
        {
            this.serializer = this.Fixture.Server.Services.GetRequiredService<IJsonSerializer>();
        }


        [Fact]
        public async Task MultiUploadFileModelWithModel()
        {
            var model = new TestModel { Property = "test", Collection = new List<string>() { "test1", "test2" } };
            var model2 = new TestModel2 { Property = "Model2Prop" };
            var model22 = new TestModel2 { Property = "Model22Prop" };
            model.ModelCollection = new List<TestModel2>() { model2, model22 };
            
            var request = this.Fixture.Server.CreateApiRequest<ValuesController>(c => c.GetPaginatedModel(null));
            var content = new MultipartFormDataContent();
            var json = await this.serializer.SerializeAsync(model);
            var modelContent = new StringContent(json, Encoding.UTF8);
            content.Add(modelContent, "model");

            request.And(message =>
            {
                message.Content = content;
            });

            var response = await request.SendAsync(HttpMethods.Post);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var data = await response.Content.ReadAsStringAsync();
            Assert.Equal(json, data);

        }


        [Fact]
        public async Task MultiUploadFileModelWithFile()
        {
            var content = new MultipartFormDataContent();
            var fileText = "sample file text content";

            var request = this.Fixture.Server.CreateApiRequest<ValuesController>(c => c.GetPaginatedFile(null));

            var fileContent = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(fileText)));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            content.Add(fileContent, "sample.txt", "sample.txt");
            request.And(message =>
            {
                message.Content = content;
            });

            var response = await request.SendAsync(HttpMethods.Post);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var data = await response.Content.ReadAsStringAsync();
            Assert.Equal(fileText, data);

        }
    }
}
