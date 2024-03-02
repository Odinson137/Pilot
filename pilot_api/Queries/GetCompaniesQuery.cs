using MediatR;
using pilot_api.DTO;
using pilot_api.Models;

namespace pilot_api.Queries;

public record GetCompaniesQuery : IRequest<ICollection<CompanyDto>>, IRequest<CompanyDto>;
