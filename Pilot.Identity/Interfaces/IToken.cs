using Pilot.Identity.Data;

namespace Pilot.Identity.Interfaces;

public interface IToken
{
    public string GenerateToken(string userId, Role role);
}