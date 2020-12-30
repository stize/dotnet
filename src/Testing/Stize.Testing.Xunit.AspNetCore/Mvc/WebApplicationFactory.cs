using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Stize.Testing.Xunit.AspNetCore.Mvc
{
    public class WebApplicationFactory<TStartUp> : IDisposable
        where TStartUp : class
    {
        
        private TestServer server;
        private IHost host;

        public ITestOutputHelper Output { get; set; }

        public TestServer Server
        {
            get
            {
                this.EnsureServer();
                return this.server;
            }
        }

        private void EnsureServer()
        {
            if (this.server != null)
            {
                return;
            }

            var hostBuilder = this.CreateHostBuilder();
            this.host = this.CreateHost(hostBuilder);
            this.server = (TestServer)this.host.Services.GetRequiredService<IServer>();

        }

        protected virtual IHost CreateHost(IHostBuilder builder)
        {
            var h = builder.Build();
            h.Start();
            return h;
        }

        protected virtual IHostBuilder CreateHostBuilder()
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
            builder
                .ConfigureWebHostDefaults(this.ConfigureWebHostDefaults)
                .ConfigureAppConfiguration(this.ConfigureAppConfiguration)
                .ConfigureLogging(this.ConfigureLogging);

            return builder;
        }

        protected virtual void ConfigureWebHostDefaults(IWebHostBuilder webBuilder)
        {
            webBuilder.UseStartup<TStartUp>()
                      .UseTestServer(); ;
        }

        protected virtual void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;
            config
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();
        }

        protected virtual void ConfigureLogging(HostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging
                .AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
                .AddXunit(this.Output);
        }

        #region IDisposable

        private bool disposed = false;

        ~WebApplicationFactory() => this.Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }


            if (disposing)
            {
                this.server?.Dispose();
                this.server = null;
                this.host?.Dispose();
                this.host = null;
                this.Output = null;
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}