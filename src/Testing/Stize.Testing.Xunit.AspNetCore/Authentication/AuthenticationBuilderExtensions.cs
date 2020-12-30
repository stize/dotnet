using Microsoft.AspNetCore.Authentication;
using Stize.Testing.Xunit.AspNetCore.Authentication;

namespace Stize.Testing.Xunit.AspNetCore.Extensions
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddTestServer(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(Constants.Scheme, null, null);
        }
    }
}