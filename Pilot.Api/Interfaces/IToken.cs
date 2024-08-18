using Pilot.Contracts.Data.Enums;

namespace Pilot.Api.Interfaces;

public interface IToken
{
    public string GenerateToken(int userId, Role role);
}