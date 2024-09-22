using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Capability.Queries;

public record GetOpenCompanyPost(BaseFilter Filter)
    : BaseQuery, IRequest<ICollection<CompanyPostDto>>
{
}

