using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Stize.Testing.Xunit
{
    public static class XunitExtensions
    {
        public static ILoggingBuilder AddXunit(this ILoggingBuilder loggingBuilder, ITestOutputHelper output)
        {
            loggingBuilder.AddProvider(new XunitProvider(output));
            return loggingBuilder;
        }
    }
}