﻿using Pilot.Contracts.DTO.ModelDto;
using Pilot.Worker.Models;
using Test.Worker.IntegrationTests.Factories;

namespace Test.Worker.IntegrationTests;

public class ProjectTaskTests : BaseModelReceiverIntegrationTest<ProjectTask, ProjectTaskDto>
{
    /// <inheritdoc />
    public ProjectTaskTests(ReceiverTestReceiverFactory receiverFactory, ReceiverTestIdentityFactory identityFactory) :
        base(receiverFactory, identityFactory)
    {
    }
}