using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class MessageTests(MessageTestCapabilityFactory factory)
    : BaseModelTest<Message, MessageDto>(factory);