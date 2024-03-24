using Moq;
using Pilot.Api.DTO;
using Pilot.Api.Handlers;
using Pilot.Api.Interfaces.Repositories;
using Pilot.Api.Queries;
using Xunit;

namespace Pilot.Tests.Api.Tests.UnitTests;

public class CompanyHandlerUnitTests
{
    private readonly CompanyQueryHandler _mockCompanyQueryHandler;
    private readonly Mock<ICompany> _mockCompanyRepository;

    public CompanyHandlerUnitTests()
    {
        _mockCompanyRepository = new Mock<ICompany>();
        _mockCompanyQueryHandler =
            new CompanyQueryHandler(_mockCompanyRepository.Object);
    }

    [Fact]
    public async Task GetCompaniesQuery_Handle_GetAllCompany_ShouldReturnOkObjectResult()
    {
        // Arrange 

        var request = new GetCompaniesQuery("TestCacheKey");

        _mockCompanyRepository.Setup(mock => 
                mock.GetCompaniesAsync(default))
                .ReturnsAsync(new List<CompanyDto>() { new CompanyDto("1", "title", "description")});

        // Act

        var companies = await _mockCompanyQueryHandler.Handle(request, default);

        //// Assert

        Assert.NotNull(companies);
        Assert.True(companies.Count > 0);
    }
    
    [Fact]
    public async Task GetCompanyByIdQuery_Handle_GetAllCompany_ShouldReturnOkObjectResult()
    {
        // Arrange 
        
        var companyId = "1";
        var request = new GetCompanyByIdQuery(companyId, "TestCacheKey");

        _mockCompanyRepository.Setup(mock => 
                mock.GetCompanyAsync(companyId, default))
            .ReturnsAsync(new CompanyDto("1", "title", "description"));

        // Act

        var company = await _mockCompanyQueryHandler.Handle(request, default);

        //// Assert

        Assert.NotNull(company);
        Assert.IsType<CompanyDto>(company);
    }
}