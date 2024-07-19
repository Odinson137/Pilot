using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Models;
using File = Pilot.Contracts.Models.File;

namespace Pilot.Receiver.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseModel, BaseDto>();
        
        CreateMap<Company, Company>();
        CreateMap<Company, CompanyDto>();
        CreateMap<CompanyDto, Company>();
        
        CreateMap<CompanyUser, CompanyUser>();
        CreateMap<CompanyUser, CompanyUserDto>();
        CreateMap<CompanyUserDto, CompanyUser>();
        
        CreateMap<File, File>();
        CreateMap<File, FileDto>();
        CreateMap<FileDto, File>();
        
        CreateMap<HistoryAction, HistoryAction>();
        CreateMap<HistoryAction, HistoryActionDto>();
        CreateMap<HistoryActionDto, HistoryAction>();
        
        CreateMap<Message, Message>();
        CreateMap<Message, MessageDto>();
        CreateMap<MessageDto, Message>();

        CreateMap<Project, Project>();
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>();
        
        CreateMap<ProjectTask, ProjectTask>();
        CreateMap<ProjectTask, ProjectTaskDto>();
        CreateMap<ProjectTaskDto, ProjectTask>();
        
        CreateMap<Team, Team>();
        CreateMap<Team, TeamDto>();
        CreateMap<TeamDto, Team>();

        
    }   
}