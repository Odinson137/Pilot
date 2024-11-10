using AutoMapper;
using Pilot.BlazorClient.ViewModels;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.BlazorClient.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseDto, BaseViewModel>();
        CreateMap<BaseViewModel, BaseDto>();
        
        Mapping<CompanyViewModel, CompanyDto>();
        Mapping<CompanyUserViewModel, CompanyUserDto>();
        Mapping<ProjectViewModel, ProjectDto>();
        Mapping<ProjectTaskViewModel, ProjectTaskDto>();
        Mapping<TeamViewModel, TeamDto>();
        Mapping<HistoryActionViewModel, HistoryActionDto>();
        Mapping<CompanyRoleViewModel, CompanyRoleDto>();
        Mapping<TaskInfoViewModel, TaskInfoDto>();
        Mapping<CompanyPostViewModel, CompanyPostDto>();
        Mapping<PostViewModel, PostDto>();
        Mapping<UserViewModel, UserDto>();
        Mapping<ChatViewModel, ChatDto>();
        Mapping<ChatMemberViewModel, ChatMemberDto>();
        Mapping<InfoMessageViewModel, InfoMessageDto>();
        Mapping<MessageViewModel, MessageDto>();
        Mapping<SkillViewModel, SkillDto>();
        Mapping<UserSkillViewModel, UserSkillDto>();
    }
    
    private void Mapping<TViewModel, TDto>() where TViewModel : BaseViewModel, new() where TDto : BaseDto
    {
        CreateMap<TViewModel?, BaseDto?>();
        CreateMap<TViewModel?, TDto?>();
        CreateMap<TDto?, TViewModel?>();
        CreateMap<TDto?, TViewModel?>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
        CreateMap<BaseDto?, TViewModel?>().ConvertUsing(src => src == null
            ? null
            : new TViewModel
            {
                Id = src.Id,
                CreateAt = src.CreateAt,
                ChangeAt = src.ChangeAt,
                DeleteAt = src.DeleteAt
            });
        CreateMap<TViewModel?, BaseDto?>().ConvertUsing(src => src == null
            ? null
            : new BaseDto
            {
                Id = src.Id,
                CreateAt = src.CreateAt,
                ChangeAt = src.ChangeAt,
                DeleteAt = src.DeleteAt
            });
    }
}