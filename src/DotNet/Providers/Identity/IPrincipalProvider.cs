using System.Security.Principal;

namespace Stize.DotNet.Providers.Identity
{
    public interface IPrincipalProvider
    {
        IPrincipal Current { get; }
    }
}