using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Stize.Testing.Xunit
{
    public class XunitProvider : ILoggerProvider
    {
        private XunitLogger logger;

        public XunitProvider(ITestOutputHelper output)
        {
            this.logger = new XunitLogger(output);
        }

        public void Dispose()
        {
            this.logger.Dispose();
            this.logger = null;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return this.logger;
        }
    }
}