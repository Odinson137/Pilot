using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class CompanyPostTests(
    ApiTestApiFactory apiFactory,
    ApiTestWorkerFactory workerFactory,
    ApiTestIdentityFactory identityFactory)
    : BaseModelIntegrationTest<CompanyPost, CompanyPostDto>(apiFactory, workerFactory, identityFactory);