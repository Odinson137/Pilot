using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Worker.Models;

public class CompanyUser : BaseModel
{
    [Required] public int UserId { get; set; }

    [Required] public Company Company { get; set; } = null!;
    
    [Required] public int PostId { get; set; }

    public List<Team> Teams { get; set; } = [];

    public List<ProjectTask> ProjectTasks { get; set; } = [];

    public CompanyRole? CompanyRole { get; set; }

    public Permission Permissions { get; set; }
}