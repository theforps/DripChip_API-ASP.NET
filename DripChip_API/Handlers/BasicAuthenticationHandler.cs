using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using DripChip_API.Domain.Enums;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IRegisterService _registerService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IRegisterService registerService)
        : base(options, logger, encoder, clock)
    {
        _registerService = registerService;
    }
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.Authorization.Any())
        {
            var claims = new[] { new Claim(ClaimTypes.Name, StatusCode.AuthorizationDataIsEmpty.ToString()) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Bad request for Authorization");
        }

        var authHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

        if (authHeaderValue.Scheme != "Basic")
        {
            return AuthenticateResult.Fail("Authorization scheme is wrong");
        }
        
        var bytes = Convert.FromBase64String(authHeaderValue.Parameter);

        var credentials = Encoding.UTF8.GetString(bytes).Split(":");

        var login = credentials[0];
        var password = credentials[1];

        if (await IsValidUser(login, password))
        {
            var claims = new[] { new Claim(ClaimTypes.Name, login) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        

        return AuthenticateResult.Fail("Login or password is wrong");
    }

    private async Task<bool> IsValidUser(string login, string password)
    {
        var result = await _registerService.GetUser(login, password);

        if (result.StatusCode == StatusCode.OK)
        {
            return true;
        }

        return false;
    }
}