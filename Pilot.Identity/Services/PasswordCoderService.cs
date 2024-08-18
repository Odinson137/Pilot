using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Services;

public class PasswordCoderService : IPasswordCoder
{
    private readonly byte[] _salt = { 80, 14, 198, 92, 132, 106, 158, 52, 83, 194, 223, 168, 28, 170, 87 };

    public string PasswordCode(string password)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password!,
            _salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));

        return hashed;
    }
}