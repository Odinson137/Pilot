namespace Pilot.Contracts.Exception.ProjectExceptions;

public class BadRequestException : System.Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}