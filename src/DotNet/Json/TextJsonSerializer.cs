using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Stize.DotNet.Json
{
    public class TextJsonSerializer : IJsonSerializer
    {
        private readonly ILogger<TextJsonSerializer> logger;
        private readonly JsonSerializerOptions settings;

        public TextJsonSerializer(ILogger<TextJsonSerializer> logger, JsonSerializerOptions settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        public async Task<string> SerializeAsync<TSource>(TSource source, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, source, source.GetType(), this.settings, cancellationToken);
                var array = stream.ToArray();
                return Encoding.UTF8.GetString(array);
            }
        }

        public async Task<TSource> DeserializeAsync<TSource>(string json, CancellationToken cancellationToken = default)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(buffer))
            {
                var obj = await JsonSerializer.DeserializeAsync<TSource>(stream, this.settings, cancellationToken);
                return obj;
            }
        }

        public async Task<object> DeserializeAsync(string json, Type type, CancellationToken cancellationToken = default)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(buffer))
            {
                var obj = await JsonSerializer.DeserializeAsync(stream, type, this.settings, cancellationToken);
                return obj;
            }
        }
    }
}
