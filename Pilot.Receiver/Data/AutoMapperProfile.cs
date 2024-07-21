using System.Reflection;
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
        CreateMap<BaseDto, BaseModel>();

        // var list = GetAllModelAndDtoType();
        //
        // foreach (var (modelType, dtoType) in list)
        // {
        //     Mapping(modelType, dtoType);
        // }

        Mapping<Company, CompanyDto>();
        Mapping<CompanyUser, CompanyUserDto>();
        Mapping<File, FileDto>();
        
        // TODO скоро удалить
        Mapping<Message, MessageDto>();
        
        Mapping<Project, ProjectDto>();
        Mapping<ProjectTask, ProjectTaskDto>();
        Mapping<Team, TeamDto>();
    }

    private void Mapping(Type modelType, Type dtoType)
    {
        CreateMap(modelType, modelType);
        CreateMap(modelType, dtoType);
        CreateMap(dtoType, modelType);
        CreateMap(typeof(BaseDto), modelType).ConvertUsing(src => CreateEntity(modelType));
    }

    private object CreateEntity(Type modelType)
    {
        var entity = (BaseModel)Activator.CreateInstance(modelType)!;
        return entity;
    }

    private void Mapping<T, TDto>() where T : BaseModel, new() where TDto : BaseDto
    {
        CreateMap<T, T>();
        CreateMap<T, TDto>();
        CreateMap<TDto, T>();
        CreateMap<BaseDto, T>().ConvertUsing(src => new T
        {
            Id = src.Id,
            CreateAt = src.CreateAt,
            ChangeAt = src.ChangeAt,
            DeleteAt = src.DeleteAt
        });
    }
    
    private ICollection<(Type, Type)> GetAllModelAndDtoType()
    {
        var baseModelType = typeof(BaseModel);
        var assemblyModel = Assembly.GetAssembly(baseModelType);

        var modelTypes = assemblyModel?.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(baseModelType))
            .OrderBy(c => c.Name)
            .ToList();

        var baseDtoType = typeof(BaseDto);
        var assemblyDto = Assembly.GetAssembly(baseDtoType);

        var dtoTypes = assemblyDto?.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(baseDtoType))
            .OrderBy(c => c.Name)
            .ToList();

        var combinedList = modelTypes?
            .Select(modelType => 
            {
                var dtoType = dtoTypes?.FirstOrDefault(dto => dto.Name == $"{modelType.Name}Dto");
                return (ModelType: modelType, DtoType: dtoType);
            })
            .Where(tuple => tuple.DtoType != null)
            .ToList();

        return combinedList!;
    }
}