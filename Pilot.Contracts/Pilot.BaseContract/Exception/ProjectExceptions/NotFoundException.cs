namespace Pilot.Contracts.Exception.ProjectExceptions;

public class NotFoundException : System.Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}