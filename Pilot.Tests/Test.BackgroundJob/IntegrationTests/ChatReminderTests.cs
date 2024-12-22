using Pilot.BackgroundJob.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.BackgroundJob.Factories;

namespace Test.BackgroundJob.IntegrationTests;

public class ChatReminderTests(
    BackgroundJobTestBackgroundJobFactory factory,
    BackgroundJobTestIdentityFactory identityFactory)
    : BaseModelReceiverIntegrationTest<ChatReminder, ChatReminderDto>(factory, identityFactory)
{
    protected override ChatReminder GetModel()
    {
        var model = base.GetModel();
        model.DayOfWeeks = [DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday];
        return model;
    }
}