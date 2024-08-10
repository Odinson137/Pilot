using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Receiver.Models;
using File = Pilot.Receiver.Models.File;

namespace Pilot.Receiver.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseModel, BaseDto>();
        CreateMap<BaseDto, BaseModel>();

        Mapping<Company, CompanyDto>();
        Mapping<CompanyUser, CompanyUserDto>();
        Mapping<File, FileDto>();
        Mapping<Project, ProjectDto>();
        Mapping<ProjectTask, ProjectTaskDto>();
        Mapping<Team, TeamDto>();
        Mapping<HistoryAction, HistoryActionDto>();
    }

    private void Mapping<T, TDto>() where T : BaseModel, new() where TDto : BaseDto
    {
        CreateMap<T?, T?>();
        CreateMap<T?, TDto?>();
        CreateMap<TDto?, T?>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
        CreateMap<BaseDto?, T?>().ConvertUsing(src => src == null ? null : new T
        {
            Id = src.Id,
            CreateAt = src.CreateAt,
            ChangeAt = src.ChangeAt,
            DeleteAt = src.DeleteAt
        });
    }
}