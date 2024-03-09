using MediatR;

namespace Pilot.Api.Commands;

public record CompanyCommand(string CompanyName, string UserId) : IRequest;
public record ChangeCompanyTitleCommand(string CompanyId, string CompanyName) : IRequest;
