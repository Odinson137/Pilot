using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class CompanyPostTests(
    ApiTestApiFactory apiFactory,
    ApiTestReceiverFactory receiverFactory,
    ApiTestIdentityFactory identityFactory)
    : BaseModelIntegrationTest<CompanyPost, CompanyPostDto>(apiFactory, receiverFactory, identityFactory);