using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class ChatTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<Chat, ChatDto>(factory, identityFactory)
{
}