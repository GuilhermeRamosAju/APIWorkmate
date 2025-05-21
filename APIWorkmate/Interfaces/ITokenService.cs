using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APIWorkmate.Interfaces;

public interface ITokenService
{
    JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
    string GenerateRefreshToken();

    ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token, IConfiguration _config);
}
