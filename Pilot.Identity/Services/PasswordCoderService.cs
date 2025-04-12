using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Pilot.Contracts.Services;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Services;

public class PasswordCoderService : IPasswordCoder
{
    public (string, string) GenerateSaltAndHashPassword(string password)
    {
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var hashed = HashedPassword(password, salt);

        return (hashed, salt.ToJson());
    }

    public string ComparePasswordAndSalt(string password, string salt)
    {
        var byteSalt = salt.FromJson<byte[]>();
        var hashed = HashedPassword(password, byteSalt);
        return hashed;
    }

    private string HashedPassword(string password, byte[] salt)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));
        
        return hashed;
    }
}