using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record UpdateCommand<TDto>(TDto ValueDto, int UserId, Guid CorrelationId) : ICommand<TDto> where TDto : BaseDto;
