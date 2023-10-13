using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Teams.Services;
public class AuthService
{
    public static string GetJwtToken(List<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
            
        var ecnodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return ecnodedJwt;
    }
}