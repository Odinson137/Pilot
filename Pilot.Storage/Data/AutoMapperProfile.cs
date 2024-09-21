using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using File = Pilot.Storage.Models.File;

namespace Pilot.Storage.Data;

public class AutoMapperProfile : BaseMappingProfile
{
    public AutoMapperProfile()
    {
        BaseMap();

        CreateMap<FileDto, File>();
        CreateMap<File, FileDto>()
            .ForMember(dest => dest.ByteFormFile, opt => opt.Ignore())
            .ForMember(dest => dest.Url, opt => opt.Ignore());
        CreateMap<BaseDto?, FileDto?>().ConvertUsing(src => src == null
            ? null
            : new FileDto
            {
                Id = src.Id,
                CreateAt = src.CreateAt,
                ChangeAt = src.ChangeAt,
                DeleteAt = src.DeleteAt
            });
        CreateMap<BaseDto?, File?>().ConvertUsing(src => src == null
            ? null
            : new File
            {
                Id = src.Id,
                CreateAt = src.CreateAt,
                ChangeAt = src.ChangeAt,
                DeleteAt = src.DeleteAt
            });
    }
}