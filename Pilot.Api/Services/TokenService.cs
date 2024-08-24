using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Pilot.Api.Interfaces;
using Pilot.Contracts.Data.Enums;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Pilot.Api.Services;

public class TokenService : IToken
{
    private readonly IConfiguration _configurationManager;

    public TokenService(IConfiguration configurationManager)
    {
        _configurationManager = configurationManager;
    }

    public string GenerateToken(int userId, Role role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configurationManager["Jwt:Key"]!);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, role.ToString())
            }),
            Expires = DateTime.Now.AddYears(999), // Вопросы безопасности во время создания этого проекта меня не волнуют
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateEncodedJwt(tokenDescriptor);
        
        //
        // var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //
        // var claims = new List<Claim>
        // {
        //     new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        //     new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //     new(ClaimTypes.Role, role.ToString())
        // };
        //
        // var token = new JwtSecurityToken(
        //     _configurationManager["Jwt:Issuer"],
        //     _configurationManager["Jwt:Issuer"],
        //     claims,
        //     expires: DateTime.Now.AddYears(999), // Вопросы безопасности во время создания этого проекта меня не волнуют
        //     signingCredentials: creeds);
        //
        // return new JwtSecurityTokenHandler().WriteToken(token);
    }
}