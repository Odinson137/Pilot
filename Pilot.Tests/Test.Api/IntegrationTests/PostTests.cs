using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class PostTests(
    ApiTestApiFactory apiFactory,
    ApiTestWorkerFactory workerFactory,
    ApiTestIdentityFactory identityFactory)
    : BaseModelIntegrationTest<Post, PostDto>(apiFactory, workerFactory, identityFactory);