using Pilot.Contracts.Base;

namespace Pilot.BackgroundJob.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();
    }
}