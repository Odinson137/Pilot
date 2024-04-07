using MediatR;

namespace Pilot.Api.Commands;

public record AddCompanyCommand(string CompanyName, string UserId) : IRequest;
public record ChangeCompanyTitleCommand(string CompanyId, string CompanyName, string UserId) : IRequest;
