using MediatR;
using pilot_api.Models;

namespace pilot_api.Commands;

public record AddCompanyCommand(string companyName) : IRequest<Company>;
