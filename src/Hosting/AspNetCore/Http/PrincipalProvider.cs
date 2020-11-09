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
        public IPrincipal Current => this.accessor.HttpContext.User;
    }
}