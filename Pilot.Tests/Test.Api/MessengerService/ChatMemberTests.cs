using Pilot.Messenger.Models;
using Test.Api.MessengerService.Factory;
using Test.Base.IntegrationBase;

namespace Test.Api.MessengerService;

[Collection(nameof(SequentialCollectionDefinition))]
public class ChatMemberTests : BaseMessengerServiceIntegrationTest
{
    public ChatMemberTests(MessengerTestApiFactory apiFactory, MessengerTestIdentityFactory identityFactory, MessengerTestMessengerFactory messengerFactory)
        : base(apiFactory, identityFactory, messengerFactory) { }

    private static string EntityName => nameof(Message);
}