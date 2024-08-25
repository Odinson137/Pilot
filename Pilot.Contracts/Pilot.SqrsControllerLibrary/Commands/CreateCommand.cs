using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Commands;

public record CreateCommand<TDto>(TDto ValueDto, int UserId) : IRequest where TDto : BaseDto;
