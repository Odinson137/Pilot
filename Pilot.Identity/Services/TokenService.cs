using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Pilot.Contracts.Data;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Services;

public class TokenService : IToken
{
    public string GenerateToken(string userId, Role role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Key));
        var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, userId),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            Jwt.Issuer,
            Jwt.Issuer,
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creeds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}