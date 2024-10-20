using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class ChatMemberTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<ChatMember, ChatMemberDto>(factory, identityFactory)
{
}