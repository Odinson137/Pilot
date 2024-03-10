using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Services;

public class PasswordCoderService : IPasswordCoder
{
    public string PasswordCode(string password)
    {
        return password;
    }
}