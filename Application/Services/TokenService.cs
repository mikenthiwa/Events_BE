using System.IdentityModel.Tokens.Jwt;
// using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Application.Common.Model;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService(JwtConfiguration configuration)
{
    public string GenerateToken(string id, string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, id),
            new Claim(JwtRegisteredClaimNames.Email, email),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: configuration.Issuer,
            audience: configuration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(configuration.ExpiryDays),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
