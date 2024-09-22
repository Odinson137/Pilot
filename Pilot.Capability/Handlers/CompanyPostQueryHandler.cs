using MediatR;
using Pilot.Capability.Interface;
using Pilot.Capability.Models;
using Pilot.Capability.Queries;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Services.LogService;
using Pilot.SqrsControllerLibrary.Handlers;

namespace Pilot.Capability.Handlers;

public class CompanyPostQueryHandler(ICompanyPost repository, ILogger<CompanyPostQueryHandler> logger) : 
    ModelQueryHandler<CompanyPost, CompanyPostDto>(repository, logger),
    IRequestHandler<GetOpenCompanyPost, ICollection<CompanyPostDto>>
{
    public async Task<ICollection<CompanyPostDto>> Handle(GetOpenCompanyPost request, CancellationToken cancellationToken)
    {
        if (request.Filter.Ids != null && !request.Filter.Ids.Any())
        {
            throw new Exception("Не передана ссылка компании");
        }
        
        var result = await repository.GetOpenPostsAsync(request.Filter, cancellationToken);
        Logger.LogClassInfo(result);
        return result;
    }
}