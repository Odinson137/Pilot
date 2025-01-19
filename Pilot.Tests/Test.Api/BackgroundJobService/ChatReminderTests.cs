using Pilot.BackgroundJob.Models;
using Pilot.Contracts.DTO.ModelDto;
using Test.Api.BackgroundJobService.Factory;

namespace Test.Api.BackgroundJobService;

public class ChatReminderTests : BackgroundJobTests<ChatReminder, ChatReminderDto>
{
    /// <inheritdoc />
    public ChatReminderTests(
        BackgroundJobTestApiFactory apiFactory, 
        BackgroundJobTestIdentityFactory identityFactory, 
        BackgroundJobTestBackgroundJobFactory backgroundJobFactory,
        BackgroundJobTestStorageFactory storageFactory)
        : base(apiFactory, identityFactory, backgroundJobFactory, storageFactory)
    {
    }
}