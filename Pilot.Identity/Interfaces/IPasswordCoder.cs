namespace Pilot.Identity.Interfaces;

public interface IPasswordCoder
{
    public (string, string) GenerateSaltAndHashPassword(string password);
    
    public string ComparePasswordAndSalt(string password, string salt);
}