using Authentication.ApiKeys.Abstractions;
using System.Security.Claims;

namespace Authentication.ApiKeys.Internal;

/// <summary>
/// Default implementation for the <see cref="IClaimsPrincipalFactory"/> service.
/// Creates a principal with the single claim of the owner ID.
/// </summary>
internal sealed class DefaultClaimsPrincipalFactory : IClaimsPrincipalFactory
{
    public Task<ClaimsPrincipal> CreateClaimsPrincipal(string apiKeyOwnerId)
    {
        var claims = new[] { new Claim(ClaimTypes.Name, apiKeyOwnerId) };
        var identity = new ClaimsIdentity(claims, ApiKeyAuthenticationOptions.DefaultScheme);
        var identities = new List<ClaimsIdentity> { identity };
        var principal = new ClaimsPrincipal(identities);

        return Task.FromResult(principal);
    }
}
