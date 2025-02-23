using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record UpdateEntityCommand<TDto>(TDto Value, int UserId) : 
    IEntityCommand<TDto> where TDto : BaseDto;