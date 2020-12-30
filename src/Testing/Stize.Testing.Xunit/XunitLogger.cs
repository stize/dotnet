using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Stize.Testing.Xunit
{
    public class XunitLogger : ILogger, IDisposable
    {
        private ITestOutputHelper output;

        public XunitLogger(ITestOutputHelper output)
        {
            this.output = output;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = string.Empty;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                //message = LogFormatter.Formatter(state, exception);
            }

            this.output.WriteLine(message);
        }

        public void Dispose()
        {
            this.output = null;
        }
    }
}