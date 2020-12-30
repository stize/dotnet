using System.Security.Principal;

namespace Stize.DotNet.Providers.Identity
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IPrincipalProvider provider;

        public IdentityProvider(IPrincipalProvider provider)
        {
            this.provider = provider;
        }

        public IPrincipal Identity => this.provider.Current;
    }
}