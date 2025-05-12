using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record DeleteCommand<TDto>(int ValueId, int UserId, Guid CorrelationId) : ICommand<int> where TDto : BaseDto
{
    public int ValueDto { get; init; }
}
