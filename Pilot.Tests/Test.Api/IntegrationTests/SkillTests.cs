using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class SkillTests(
    ApiTestApiFactory apiFactory,
    ApiTestReceiverFactory receiverFactory,
    ApiTestIdentityFactory identityFactory)
    : BaseModelIntegrationTest<Skill, SkillDto>(apiFactory, receiverFactory, identityFactory);