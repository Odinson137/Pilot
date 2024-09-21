using MediatR;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Capability.Queries;

public record GetOpenCompanyPost<TDto>(BaseFilter Filter)
    : BaseQuery, IRequest<TDto> where TDto : CompanyPostDto
{
}

