using MediatR;
using Pilot.Api.DTO;

namespace Pilot.Api.Queries;

public record GetCompaniesQuery : IRequest<ICollection<CompanyDto>>, IRequest<CompanyDto>;
