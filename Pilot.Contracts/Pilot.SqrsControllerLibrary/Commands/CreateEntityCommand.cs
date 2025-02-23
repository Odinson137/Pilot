using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record CreateEntityCommand<TDto>(TDto Value, int UserId) : 
    IEntityCommand<TDto> where TDto : BaseDto;