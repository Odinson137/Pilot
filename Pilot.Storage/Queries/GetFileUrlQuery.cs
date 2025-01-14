using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.Storage.Queries;

public record GetFileUrlQuery<TDto>(string Name)
    : BaseQuery, IRequest<TDto> where TDto : BaseDto;

