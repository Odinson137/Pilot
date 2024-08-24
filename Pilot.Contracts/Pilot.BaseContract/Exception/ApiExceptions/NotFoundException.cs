namespace Pilot.Contracts.Exception.ApiExceptions;

public class NotFoundException : System.Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}