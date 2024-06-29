using MediatR;

namespace Pilot.Api.Commands;

public record AddValueCommand<TDto>(TDto ValueDto, string UserId) : IRequest;
public record UpdateValueCommand<TDto>(TDto ValueDto, string UserId) : IRequest;