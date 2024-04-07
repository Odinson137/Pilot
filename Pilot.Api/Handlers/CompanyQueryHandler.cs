using MediatR;
using Pilot.Api.Queries;
using Pilot.Contracts.DTO;
using Pilot.Contracts.Exception.ProjectExceptions;

namespace Pilot.Api.Handlers;

public class CompanyQueryHandler : 
    IRequestHandler<GetCompaniesQuery, ICollection<CompanyDto>>,
    IRequestHandler<GetCompanyByIdQuery, CompanyDto>
{
    private readonly HttpClient _httpClient;

    public CompanyQueryHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ReceiverServer");
    }
    
    public async Task<ICollection<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync("/Company", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }
        var list = await response.Content.ReadFromJsonAsync<ICollection<CompanyDto>>(cancellationToken);
        return list!;
    }

    public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"/Company/{request.Id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new BadRequestException(await response.Content.ReadAsStringAsync(cancellationToken));   
        }
        var company = await response.Content.ReadFromJsonAsync<CompanyDto>(cancellationToken);
        return company!;
    }
}