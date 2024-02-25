using MediatR;
using pilot_api.Models;

namespace pilot_api.Queries;

public record GetCompanyQuery : IRequest<ICollection<Company>>;
