namespace Pilot.Contracts.Exception.ApiExceptions;

public class BadRequestException : System.Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}