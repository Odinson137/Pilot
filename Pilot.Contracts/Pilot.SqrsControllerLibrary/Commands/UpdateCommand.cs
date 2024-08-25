using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Commands;

public record UpdateCommand<TDto>(TDto ValueDto, int UserId) : IRequest where TDto : BaseDto;
