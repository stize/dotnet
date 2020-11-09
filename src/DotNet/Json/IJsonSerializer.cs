using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stize.DotNet.Json
{
    public interface IJsonSerializer
    {
        Task<string> SerializeAsync<TSource>(TSource source, CancellationToken cancellationToken = default);
        Task<TSource> DeserializeAsync<TSource>(string json, CancellationToken cancellationToken = default);
        Task<object> DeserializeAsync(string json, Type type, CancellationToken cancellationToken = default);
    }
}