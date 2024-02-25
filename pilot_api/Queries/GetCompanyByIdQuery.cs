using MediatR;
using pilot_api.Models;

namespace pilot_api.Queries;

public record GetCompanyByIdQuery(string Id) : IRequest<Company>;

