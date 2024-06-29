using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Services;

public class TokenService : IToken
{
    private readonly IConfiguration _configurationManager;

    public TokenService(IConfiguration configurationManager)
    {
        _configurationManager = configurationManager;
    }

    public string GenerateToken(int userId, Role role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager["Jwt:Key"]!));
        var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, userId.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Role, role.ToString())
        };

        var token = new JwtSecurityToken(
            _configurationManager["Jwt:Issuer"],
            _configurationManager["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creeds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}