using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;
using Test.Messenger.IntegrationTests.Factories;

namespace Test.Messenger.IntegrationTests;

public class InfoMessageTests(MessageTestMessageFactory factory, MessageTestIdentityFactory identityFactory)
    : BaseModelTest<InfoMessage, InfoMessageDto>(factory, identityFactory)
{
    // У InfoMessage не будет возможности изменять сообщение и удалять их, только создание
    public override Task UpdateModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }
    
    public override Task DeleteModelTest_ReturnOk()
    {
        return Task.CompletedTask;
    }
}