using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Stize.DotNet.Providers.Identity;

namespace Stize.Hosting.AspNetCore.Http
{
    public class PrincipalProvider : IPrincipalProvider
    {
        private readonly IHttpContextAccessor accessor;

        public PrincipalProvider(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }
        public IPrincipal Current => this.accessor.HttpContext?.User;

        /// <summary>
        /// Gets a value indicating whether the current IPrincipal object has a valid value.
        /// </summary>
        public bool HasValue => this.accessor != null && this.accessor.HttpContext != null && this.accessor.HttpContext.User != null;

    }
}