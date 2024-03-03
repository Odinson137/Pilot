using MediatR;
using Pilot.Api.DTO;

namespace Pilot.Api.Queries;

public record GetCompanyByIdQuery(string Id) : IRequest<CompanyDto>;

