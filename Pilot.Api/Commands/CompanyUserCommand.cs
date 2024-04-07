using MediatR;

namespace Pilot.Api.Commands;

public record AddCompanyUserCommand(string UserId, string AuthorId, string CompanyId) : IRequest;
