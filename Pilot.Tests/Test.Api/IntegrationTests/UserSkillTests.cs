﻿using Pilot.Capability.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class UserSkillTests(
    ApiTestApiFactory apiFactory,
    ApiTestWorkerFactory workerFactory,
    ApiTestIdentityFactory identityFactory)
    : BaseModelIntegrationTest<UserSkill, UserSkillDto>(apiFactory, workerFactory, identityFactory);