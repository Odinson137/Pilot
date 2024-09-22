using AutoMapper;
using Pilot.Contracts.Services;

namespace Pilot.Contracts.Base;

public abstract class BaseMappingProfile : Profile
{
    protected void BaseMap()
    {
        CreateMap<BaseModel, BaseDto>();
        CreateMap<BaseDto, BaseModel>();
    }
    
    protected void Mapping<T, TDto>() where T : BaseModel, new() where TDto : BaseDto
    {
        CreateMap<T?, T?>();
        CreateMap<T?, TDto?>();
        CreateMap<TDto?, T?>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
        CreateMap<BaseDto?, T?>().ConvertUsing(src => src == null
            ? null
            : new T
            {
                Id = src.Id,
                CreateAt = src.CreateAt,
                ChangeAt = src.ChangeAt,
                DeleteAt = src.DeleteAt
            });
    }
}