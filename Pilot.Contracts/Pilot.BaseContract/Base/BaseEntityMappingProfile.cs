using AutoMapper;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Contracts.Base;

public abstract class BaseEntityMappingProfile<T, TDto> : Profile where T : BaseModel, new() where TDto : BaseDto
{
    public BaseEntityMappingProfile()
    {
        Mapping();
    }

    private void Mapping()
    {
        EntityToEntity();
        EntityToDto();
        DtoToEntity();
        BaseDtoToEntity();

        // CreateMap<string?, FileDto?>().ConvertUsing<StringToFileDtoConverter>();
        // CreateMap<FileDto?, string?>().ConvertUsing<FileDtoToStringConverter>();
        //
        // CreateMap<List<string>?, List<FileDto>?>().ConvertUsing((src, dest, ctx) =>
        //     src == null ? null : src.Select(img => ctx.Mapper.Map<FileDto>(img)).ToList());
        // CreateMap<List<FileDto>?, List<string>?>().ConvertUsing((src, dest, ctx) =>
        //     src == null ? null : src.Select(dto => ctx.Mapper.Map<string>(dto)).ToList());
        // StringToFileDto();
    }

    protected virtual IMappingExpression<T?, T?> EntityToEntity()
    {
        return CreateMap<T?, T?>();
    }

    protected virtual IMappingExpression<T?, TDto?> EntityToDto()
    {
        return CreateMap<T?, TDto?>();
    }

    protected virtual IMappingExpression<TDto?, T?> DtoToEntity()
    {
        var map = CreateMap<TDto?, T?>();
        map.ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
        return map;
    }

    protected virtual IMappingExpression<BaseDto?, T?> BaseDtoToEntity()
    {
        var map = CreateMap<BaseDto?, T?>();
        map.ConvertUsing(src => src == null
            ? null
            : new T
            {
                Id = src.Id,
                CreateAt = src.CreateAt,
                ChangeAt = src.ChangeAt,
                DeleteAt = src.DeleteAt
            });
        return map;
    }

    protected virtual IMappingExpression<string?, TDto?> StringToFileDto()
    {
        return CreateMap<string?, TDto?>()
            .ForMember(c => c, x =>
            {
                x.Condition(p => p != null);
                x.MapFrom(p => new FileDto { Name = p! });
            });
    }
}
