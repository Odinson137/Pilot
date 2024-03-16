namespace Pilot.Identity.Interfaces;

public interface IPasswordCoder
{
    public string PasswordCode(string password);
}