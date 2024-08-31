using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();

        Mapping<File, FileDto>();
    }
}