using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record DeleteEntityCommand<TDto>(int Value, int UserId) :
    IEntityCommand<int> where TDto : BaseDto
{
}