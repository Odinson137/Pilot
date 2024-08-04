namespace Pilot.Contracts.Base;

public interface IBaseUser
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
}