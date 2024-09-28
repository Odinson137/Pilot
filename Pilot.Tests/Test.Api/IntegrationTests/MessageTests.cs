using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Api.IntegrationTests.Factories;

namespace Test.Api.IntegrationTests;

public class MessageTests(
    ApiTestApiFactory apiFactory,
    ApiTestWorkerFactory workerFactory,
    ApiTestIdentityFactory identityFactory)
    : BaseModelIntegrationTest<Message, MessageDto>(apiFactory, workerFactory, identityFactory);