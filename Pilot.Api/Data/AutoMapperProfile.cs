using AutoMapper;
using Pilot.Api.DTO;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Api.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseModel, BaseDto>();
        
        CreateMap<Company, CompanyDto>();
        CreateMap<CompanyDto, Company>();
        
        CreateMap<CompanyUser, CompanyUserDto>();
        CreateMap<CompanyUserDto, CompanyUser>();
        
        CreateMap<File, FileDto>();
        CreateMap<FileDto, File>();
        
        CreateMap<HistoryAction, HistoryActionDto>();
        CreateMap<HistoryActionDto, HistoryAction>();
        
        CreateMap<Message, MessageDto>();
        CreateMap<MessageDto, Message>();

        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>();
        
        CreateMap<ProjectTask, ProjectTaskDto>();
        CreateMap<ProjectTaskDto, ProjectTask>();
        
        CreateMap<Team, TeamDto>();
        CreateMap<TeamDto, Team>();

        
    }   
}