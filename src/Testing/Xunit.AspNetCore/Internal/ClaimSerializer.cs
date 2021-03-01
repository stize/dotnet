using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Stize.Testing.Xunit.AspNetCore.Authentication
{
    internal class ClaimSerializer
    {
        public static string Encode(IEnumerable<Claim> claims)
        {
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims)), Constants.Scheme);

            var serializer = new TicketSerializer();
            var bytes = serializer.Serialize(ticket);

            return Convert.ToBase64String(bytes);
        }

        public static IEnumerable<Claim> Decode(string encodedValue)
        {
            if (string.IsNullOrEmpty(encodedValue))
            {
                return Enumerable.Empty<Claim>();
            }

            var serializer = new TicketSerializer();
            try
            {
                var ticket = serializer.Deserialize(Convert.FromBase64String(encodedValue));

                return ticket.Principal.Claims;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Claim>();
            }
        }
    }
}