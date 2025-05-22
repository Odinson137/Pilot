using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;


public record CreateCommand<TDto>(TDto ValueDto, int UserId, Guid CorrelationId) : ICommand<TDto> where TDto : BaseDto;

