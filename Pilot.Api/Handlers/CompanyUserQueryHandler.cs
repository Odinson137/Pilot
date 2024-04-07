using MediatR;
using Pilot.Api.Queries;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Api.Handlers;

public class CompanyUserQueryHandler :
    IRequestHandler<GetCompanyUsersQuery, ICollection<CompanyUserDto>>
{
    private readonly HttpClient _httpClient;

    public CompanyUserQueryHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ReceiverServer");
    }
    
    public async Task<ICollection<CompanyUserDto>> Handle(GetCompanyUsersQuery request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"CompanyUser/{request.CompanyId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }
        var list = await response.Content.ReadFromJsonAsync<ICollection<CompanyUserDto>>(cancellationToken);
        return list!;
    }
}