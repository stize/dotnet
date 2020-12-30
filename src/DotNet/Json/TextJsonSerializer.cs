using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Stize.DotNet.Json
{
    public class TextJsonSerializer : IJsonSerializer
    {
        private readonly ILogger<TextJsonSerializer> logger;
        private readonly JsonSerializerOptions options;

        public TextJsonSerializer(ILogger<TextJsonSerializer> logger, IOptions<JsonSerializerOptions> options)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        public string Serialize<TSource>(TSource source)
        {
            return JsonSerializer.Serialize(source, source.GetType(), this.options);
        }

        public async Task<string> SerializeAsync<TSource>(TSource source, CancellationToken cancellationToken = default)
        {
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, source, source.GetType(), this.options, cancellationToken);
                var array = stream.ToArray();
                return Encoding.UTF8.GetString(array);
            }
        }

        public async Task<TSource> DeserializeAsync<TSource>(string json, CancellationToken cancellationToken = default)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(buffer))
            {
                var obj = await JsonSerializer.DeserializeAsync<TSource>(stream, this.options, cancellationToken);
                return obj;
            }
        }

        public async Task<TSource> DeserializeAsync<TSource>(Stream stream, CancellationToken cancellationToken = default)
        {
            var obj = await JsonSerializer.DeserializeAsync<TSource>(stream, this.options, cancellationToken);
            return obj;
        }

        public async Task<object> DeserializeAsync(string json, Type type, CancellationToken cancellationToken = default)
        {
            var buffer = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(buffer))
            {
                var obj = await JsonSerializer.DeserializeAsync(stream, type, this.options, cancellationToken);
                return obj;
            }
        }

        public async Task<object> DeserializeAsync(Stream stream, Type type, CancellationToken cancellationToken = default)
        {
            var obj = await JsonSerializer.DeserializeAsync(stream, type, this.options, cancellationToken);
            return obj;
        }
    }
}
