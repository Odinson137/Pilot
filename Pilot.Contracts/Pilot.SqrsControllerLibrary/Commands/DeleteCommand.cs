using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record DeleteCommand<TDto>(int ValueId, int UserId) : ICommand<int> where TDto : BaseDto
{
    public int ValueDto { get; init; }
}
