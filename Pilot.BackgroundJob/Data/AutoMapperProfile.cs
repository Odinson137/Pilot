using Pilot.BackgroundJob.Models;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BackgroundJob.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();
        Mapping<ChatReminder, ChatReminderDto>();
    }
}