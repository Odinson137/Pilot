using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Interfaces;

namespace Pilot.SqrsControllerLibrary.Commands;

public record UpdateCommand<TDto>(TDto ValueDto, int UserId, Guid SagaId) : ICommand<TDto> where TDto : BaseDto;
