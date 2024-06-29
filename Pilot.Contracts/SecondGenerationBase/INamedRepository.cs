namespace Pilot.Contracts.SecondGenerationBase;

public interface INamedRepository
{
    public Task<bool> IsNameExistedAsync(string name);
}