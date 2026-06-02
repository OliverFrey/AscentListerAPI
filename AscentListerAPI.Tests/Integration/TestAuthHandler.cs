using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AscentListerAPI.Tests.Integration;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, loggerFactory, encoder)
{
    public const string SchemeName = "Test";
    public const string HeaderName = "X-Test-Auth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderName))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, "test-user") },
            SchemeName);
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(identity),
            SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
