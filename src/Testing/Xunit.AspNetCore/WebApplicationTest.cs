using System;
using Stize.Testing.Xunit.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace Stize.Testing.Xunit.AspNetCore
{
    public class WebApplicationTest<TStartUp> : IClassFixture<WebApplicationFactory<TStartUp>>, IDisposable where TStartUp : class
    {
        public WebApplicationTest(WebApplicationFactory<TStartUp> fixture, ITestOutputHelper output)
        {
            this.Fixture = fixture;
            fixture.Output = output;
        }

        public WebApplicationFactory<TStartUp> Fixture { get; private set; }

        #region IDisposable

        private bool disposed = false;

        ~WebApplicationTest() => this.Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }


            if (disposing)
            {
                this.Fixture.Output = null;
                this.Fixture = null;
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