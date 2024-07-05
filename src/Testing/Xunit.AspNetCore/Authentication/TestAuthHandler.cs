using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Stize.Testing.Xunit.AspNetCore.Authentication
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (this.Context.Request.Headers.TryGetValue(Constants.AuthorizationHeaderName, out var authorizationHeader))
            {
                var authorization = authorizationHeader.ToString();
                if (authorization.StartsWith($"{this.Scheme.Name} ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authorization.Substring($"{this.Scheme.Name} ".Length).Trim();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var claims = ClaimSerializer.Decode(token);
                        var identity = new ClaimsIdentity(claims, Constants.Scheme);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Constants.Scheme);
                        var result = AuthenticateResult.Success(ticket);
                        return Task.FromResult(result);
                    }

                }
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}