using System.ComponentModel;
using Pilot.Contracts.DTO.ModelDto;

namespace Pilot.Contracts.Data.Enums;

/// <summary>
///     Этот enum требуется, чтоб классифицировать сущности в бд. Например, в модели Message
///     Используется Dto вместо ссылок на реальные модели, чтобы не делать ссылки из контрактов на все другие проекты
/// </summary>
public enum ModelType
{
    NotSelected = 0,
    
    [Description(nameof(UserDto))] User = 1,

    [Description(nameof(CompanyDto))] Company = 2,

    [Description(nameof(CompanyUserDto))] CompanyUser = 3,

    [Description(nameof(ProjectDto))] Project = 4,

    [Description(nameof(FileDto))] File = 5,

    [Description(nameof(InfoMessageDto))] InfoMessage = 6,

    [Description(nameof(ProjectTaskDto))] ProjectTask = 7,

    [Description(nameof(TeamDto))] Team = 8,

    [Description(nameof(HistoryActionDto))] HistoryAction = 9,
    
    [Description(nameof(SkillDto))] Skill = 10,
    
    [Description(nameof(PostDto))] Post = 11,
    
    [Description(nameof(TaskInfoDto))] TaskInfo = 12,
    
    [Description(nameof(ChatDto))] Chat = 13,
    
    [Description(nameof(MessageDto))] Message = 14,
    
    [Description(nameof(ChatMemberDto))] ChatMember = 15,

    [Description(nameof(ChatReminderDto))] ChatReminder = 16,
    
    [Description(nameof(TeamEmployeeDto))] TeamEmployee = 17,
}