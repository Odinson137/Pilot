using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Commands;

public record DeleteCommand<TDto>(int ValueId, int UserId) : IRequest where TDto : BaseDto;
