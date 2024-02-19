using MediatR;
using pilot_api.Models;

namespace pilot_api.Queries;

public record GetCompaniesQuery : IRequest<ICollection<Company>>;
