using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.TestHost;
using Stize.Testing.Xunit.AspNetCore.Authentication;

namespace Stize.Testing.Xunit.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder WithIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims)
        {
            requestBuilder.AddHeader(
                Constants.AuthorizationHeaderName,
                $"{Constants.Scheme} {ClaimSerializer.Encode(claims)}");

            return requestBuilder;
        }


    }
}