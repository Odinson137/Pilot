using MediatR;
using pilot_api.DTO;

namespace pilot_api.Queries;

public record GetCompanyByIdQuery(string Id) : IRequest<CompanyDto>;

