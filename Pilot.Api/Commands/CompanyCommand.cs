using MediatR;

namespace Pilot.Api.Commands;

public record CompanyAddCommand(string CompanyName, string UserId) : IRequest;
public record ChangeCompanyTitleCommand(string CompanyId, string CompanyName, string UserId) : IRequest;
