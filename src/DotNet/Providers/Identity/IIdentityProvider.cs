using System.Security.Principal;

namespace Stize.DotNet.Providers.Identity
{
    public interface IIdentityProvider
    {
        IPrincipal Identity { get; }
    }
}