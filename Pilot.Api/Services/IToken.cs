
using Pilot.Contracts.Data.Enums;

namespace Pilot.Api.Services;

public interface IToken
{
    public string GenerateToken(int userId, Role role);
}