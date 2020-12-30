using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.DotNet.Json
{
    public interface IJsonSerializer
    {
        string Serialize<TSource>(TSource source);
        Task<string> SerializeAsync<TSource>(TSource source, CancellationToken cancellationToken = default);

        Task<TSource> DeserializeAsync<TSource>(string json, CancellationToken cancellationToken = default);
        Task<TSource> DeserializeAsync<TSource>(Stream stream, CancellationToken cancellationToken = default);

        Task<object> DeserializeAsync(string json, Type type, CancellationToken cancellationToken = default);
        Task<object> DeserializeAsync(Stream stream, Type type, CancellationToken cancellationToken = default);
    }
}