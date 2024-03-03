using MediatR;
using Pilot.Api.Models;

namespace Pilot.Api.Commands;

public record AddCompanyCommand(string CompanyName) : IRequest<Company>;
